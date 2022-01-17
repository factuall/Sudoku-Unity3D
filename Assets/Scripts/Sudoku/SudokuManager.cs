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

    public int currentField = 0;

    int[,] Solve = new int[9, 9];

    // Start is called before the first frame update
    void Start()
    {
        int[,] Solution = Generator();
        Solve = Solution;
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
            if (notSolved > 38) unsolving = false;
        }

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
                GameFields[updateY * 9 + updateX].content = Solve[updateX, updateY];
            }
        }
    }

    int SetXY(int x, int y)
    {
        return x + (y * 9);
    }

    public void FieldPressed(int id)
    {
        GameFields[currentField].FieldImage.color = new Color(1f, 1f, 1f, 1f);
        currentField = id;
        GameFields[id].FieldImage.color = new Color(0.8f, 0.8f, 0.8f, 1f);
    }

    public void NumberPressed(int id)
    {
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                if (y * 9 + x == currentField) Solve[x, y] = id + 1; 
            }
        }
     
    }

    int[,] Generator()
    {
        int[] Fields = new int[81];
        int[] FirstRow = RandomOrder9();
        Array.Copy(FirstRow, Fields, 9);
        int[] CurrentShift = FirstRow;
        int[][] RestRows = new int[9][];
        for (int row = 0; row < 9; row++)
        {
            CurrentShift = ShiftArray(CurrentShift);
            RestRows[row] = CurrentShift;
        }
        int[] RowsOrder = RandomOrder9();
        for (int row = 0; row < 9; row++)
        {
            Array.Copy(RestRows[RowsOrder[row]-1], 0, Fields, row * 9, 9);
        }
        int[,] Solution = new int[9, 9];
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                Solution[x, y] = Fields[y * 9 + x];
            }
        }
        return Solution;
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
