using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SudokuManager : MonoBehaviour
{

    public RectTransform Grid;
    public NumberField[] GameFields = new NumberField[81];
    public GameObject GFPrefab;

    public RectTransform ActionGrid;
    public NumberAction[] NumberSelect = new NumberAction[9];
    public GameObject NSPrefab;

    Color highlitedColor = new Color(0.75f, 0.75f, 0.75f, 1f);
    Color highlitedNumberColor = new Color(0.6f, 0.75f, 0.75f, 1f);
    Color normalColor = new Color(1f, 1f, 1f, 155f / 255f);

    Color fontNormalColor = Color.black;
    Color fontGhostColor = new Color(0.64f, 0.64f, 0.64f, 1f);
    Color fontHighlitedColor = new Color(0.5f, 0f, 0.5f, 1f);
    Color fontCorrectAnswer = new Color(0.2f, 0.55f, 0.2f, 1f);

    public int currentField = -1;

    int[,] Solve = new int[9, 9];
    int[,] Solution = new int[9, 9];
    int[,] StartingSolved = new int[9, 9];

    // Start is called before the first frame update
    void Start()
    {
        Solution = Generator();
        Solve = Solution.Clone() as int[,];
        bool unsolving = true;
        while (unsolving)
        {
            int[] delX = RandomOrder9();
            int[] delY = RandomOrder9();
            for (int i = 0; i < 9; i++)
            {

                Solve[delX[i] - 1, delY[i] - 1] = 0;
            }
            int notSolved = 0;
            for (int y = 0; y < 9; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    if (Solve[x, y] == 0) notSolved++;
                }
            }
            if (notSolved > 30) unsolving = false;
        
        }
        StartingSolved = Solve.Clone() as int[,];
        for (int i = 0; i < 81; i++)
        {
            if(i < 9)
            {
                NumberSelect[i] = Instantiate(NSPrefab, ActionGrid).GetComponent<NumberAction>();
                NumberSelect[i].Manager = this;
                NumberSelect[i].id = i;
            }
            GameFields[i] = Instantiate(GFPrefab, Grid).GetComponent<NumberField>();
            GameFields[i].Manager = this;
            GameFields[i].id = i;
            //GameFields[i].content = Solution[i];
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        for (int updateY = 0; updateY < 9; updateY++)
        {
            for (int updateX = 0; updateX < 9; updateX++)
            {
                
                if(GameFields[updateY * 9 + updateX].ghost)
                {
                    GameFields[updateY * 9 + updateX].FieldText.color = fontGhostColor;
                }
                else
                {
                    GameFields[updateY * 9 + updateX].content = Solve[updateX, updateY];
                    GameFields[updateY * 9 + updateX].FieldText.color = (Solve[updateX, updateY] == Solution[updateX, updateY] &&
                        Solve[updateX, updateY] != StartingSolved[updateX, updateY])
                            ? fontCorrectAnswer  : fontNormalColor;
                    GameFields[updateY * 9 + updateX].FieldImage.color = normalColor;
                }
                
            }
        }
        if (currentField != -1)
        {
            HighlightField(true, currentField % 9, currentField / 9);
        }
        for (int id = 0; id < 81; id++)
        {
            if (currentField != -1 && GameFields[id].content == highlightNumberBuffer && highlightNumberBuffer != 0)
            {
                GameFields[id].FieldImage.color = highlitedNumberColor;
                GameFields[id].FieldText.color = fontHighlitedColor;

            }   
            
        }
    }

    int SetXY(int x, int y)
    {
        return x + (y * 9);
    }

    int undoBuffer = -1;

    public void FieldPressed(int id)
    {   
        
        if(currentField != -1)
        {

            HighlightField(false, currentField % 9, currentField / 9);
            GameFields[currentField].ghost = false;
            
        }
        if (id != -1)
        {

            highlightNumberBuffer = GameFields[id].content;
            if (currentField != -1)
            {
                if (GameFields[currentField].ghost)
                {
                    Solve[currentField % 9, currentField / 9] = GameFields[currentField].content;
                    GameFields[currentField].ghost = false;
                }
            }
            undoBuffer = Solve[id % 9, id / 9];
        }
            
        currentField = id;
        
    }

    public void HighlightField(bool newHighlight, int x, int y)
    {
        GameFields[y * 9 + x].FieldImage.color = newHighlight ? highlitedColor : normalColor;
        int squareX = x / 3, squareY = y / 3;
        for (int id = 0; id < 81; id++)
        {
            
            if(id % 9 == x ||
                id / 9 == y ||
                ((id % 9) / 3 == squareX&&(id / 9) / 3 == squareY)) GameFields[id].FieldImage.CrossFadeColor(newHighlight ? highlitedColor : normalColor, 0.1f, true, true);

        }
    }


    int highlightNumberBuffer = -1;
    public void NumberPressed(int id)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            { 
                if (y * 9 + x == currentField && !GameFields[currentField].ghost) Solve[x, y] = id + 1;
                else if(currentField != -1) GameFields[currentField].content = id + 1;
                highlightNumberBuffer = id+1;
            }
        }
     
    }

    public void CancelPressed()
    {
        if(currentField != -1 && undoBuffer != -1)
        {
            HighlightField(false, currentField % 9, currentField / 9);
            Solve[currentField % 9, currentField / 9] = undoBuffer;
            currentField = -1;
            undoBuffer = -1;
        }
    }

    public void GhostPressed()
    {
        int prevField = currentField;
        FieldPressed(-1);
        GameFields[prevField].ghost = true;

    }

    int[,] Generator()
    {
        int[] Fields = new int[81];
        int[] FirstRow = RandomOrder9();
        Array.Copy(FirstRow, Fields, 9);
        int[] CurrentShift = FirstRow;
        int[][] RestRows = new int[9][];
        for (int row = 1; row < 9; row++)
        {
            if(row != 0 && row != 3 && row != 6)
            {
                CurrentShift = ShiftArray(CurrentShift);
                CurrentShift = ShiftArray(CurrentShift);
                CurrentShift = ShiftArray(CurrentShift);
            }
            else
            {
                CurrentShift = ShiftArray(CurrentShift);

            }
            
            RestRows[row] = CurrentShift;
        }
        RestRows[0] = FirstRow;
        int[] RowsOrder = {6,7,8,3,4,5,0,1,2};
        for (int row = 0; row < 9; row++)
        {
            Array.Copy(RestRows[RowsOrder[row]], 0, Fields, row * 9, 9);
        }
        int[,] sol = new int[9, 9];
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                sol[x, y] = Fields[y * 9 + x];
            }
        }
        return sol;
    }

    int[] RandomOrder9()
    {
        int[] Fields = new int[9];
        for (int frField = 0; frField < 9; frField++)
        {
            bool duplicate = true;
            int fieldValue = 0;
            while (duplicate)
            {
                fieldValue = (int)Mathf.Round(UnityEngine.Random.Range(0.5f, 9.49f));
                duplicate = false;
                for (int previous = 0; previous < frField; previous++)
                {
                    if (fieldValue == Fields[previous]) duplicate = true;
                }
            }
            Fields[frField] = fieldValue;
        }
        return Fields;
    }

    int[] ShiftArray(int[] input)
    {
        int firstValue = input[0];
        var shifted = new int?[input.Length];
        Array.Copy(input, 1, shifted, 0, input.Length - 1);
        shifted[shifted.Length - 1] = firstValue;
        int[] result = Array.ConvertAll(shifted, x => x ?? 0);
        return result;
    }


}
