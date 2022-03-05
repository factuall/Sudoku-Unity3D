using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SudokuManager : MonoBehaviour
{

    public static float playTime = 0;
    public static int playScore = 0;
    public static int playMistakes = 0;

    public RectTransform Grid;
    public NumberField[] GameFields = new NumberField[81];
    public GameObject GFPrefab;

    public RectTransform ActionGrid;
    public NumberAction[] NumberSelect = new NumberAction[9];
    public GameObject NSPrefab;

    public Color tint;

    Color highlitedColor = new Color(0.75f, 0.75f, 0.75f, 1);
    Color highlitedNumberColor = new Color(0.6f, 0.75f, 0.75f, 155f / 255f);
    Color normalColor = new Color(1f, 1f, 1f, 155f / 255f);

    Color fontNormalColor = Color.black;
    Color fontGhostColor = new Color(0.64f, 0.64f, 0.64f, 1f);
    Color fontHighlitedColor = new Color(0.5f, 0f, 0.5f, 1f);
    Color fontCorrectAnswer = new Color(0.2f, 0.55f, 0.2f, 1f);
    Color fontMistakeColor = new Color(0.8f, 0, 0);

    public int currentField = -1;

    public Color[] randomColors = new Color[80];

    int difficulty = SudokuLauncher.launchDifficulty;

    int[,] Solve = new int[9, 9];
    int[,] Solution = new int[9, 9];
    int[,] StartingSolved = new int[9, 9];

    int[,] modeKillerGrid = new int[9, 9];

    bool gameReady = false;
    bool sumMode = false;

    // Start is called before the first frame update
    void Start()
    {
        playTime = 0;
        Solution = Generator();
        Solve = Solution.Clone() as int[,];
        bool unsolving = true;
        int notSolved = 0;
        while (unsolving)
        {
            int[] delX = RandomOrder9();
            int[] delY = RandomOrder9();
            for (int i = 0; i < 9; i++)
            {
                Solve[delX[i] - 1, delY[i] - 1] = 0;
                notSolved = 0;
                for (int y = 0; y < 9; y++)
                {
                    for (int x = 0; x < 9; x++)
                    {
                        if (Solve[x, y] == 0) notSolved++;
                    }
                }
                if (notSolved > difficulty - 1) 
                {
                    unsolving = false;
                    break;
                }
            }
        
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

        for (int color = 0; color < 80; color++)
        {
            randomColors[color] = randomColor();
        }
        if(sumMode) modeKillerGrid = modeKiller();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameReady = !(GameFields[0].FieldText == null);
        if (!gameReady) return; //update fields appearance 
        playTime += Time.deltaTime;
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
                    GameFields[updateY * 9 + updateX].FieldText.color = Color.black;
                    if(Solve[updateX, updateY] != StartingSolved[updateX, updateY])
                    {
                        if (Solve[updateX, updateY] == Solution[updateX, updateY])
                        {
                            GameFields[updateY * 9 + updateX].FieldText.color = fontCorrectAnswer;
                        }
                        else
                        {
                            GameFields[updateY * 9 + updateX].FieldText.color = fontMistakeColor;
                        }
                    }
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
            if (sumMode)
            {
                GameFields[id].fieldTintImage.color = randomColors[modeKillerGrid[id % 9, id / 9]];
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
            if (!GameFields[currentField].ghost &&
                Solve[currentField % 9, currentField / 9] != StartingSolved[currentField % 9, currentField / 9] &&
                Solve[currentField % 9, currentField / 9] != Solution[currentField % 9, currentField / 9]) 
                    playMistakes++;
        }
        if (id != -1)
        {
            highlightNumberBuffer = GameFields[id].content;
            undoBuffer = Solve[id % 9, id / 9];
        }

        currentField = id;

        //solved check
        for (int i = 0; i < 81; i++)
        {
            if (Solve[i % 9, i / 9] != Solution[i % 9, i / 9] || GameFields[i].ghost) return;
        }

        SceneManager.LoadScene("Winscreen");

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
                if (y * 9 + x == currentField) Solve[x, y] = id + 1;
                else if(currentField != -1) GameFields[currentField].content = id + 1;
                highlightNumberBuffer = id+1;
            }
        }
     
    }

    public void AcceptPressed()
    {
        if (currentField != -1) {
            GameFields[currentField].ghost = false;
            FieldPressed(-1);
            
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
        int isntMistake = playMistakes;
        int prevField = currentField;
        if(currentField != -1)
        {
            GameFields[prevField].ghost = true;
        }
        FieldPressed(-1);
        GameFields[prevField].ghost = true;
        playMistakes = isntMistake; 
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

    public void QuitSudoku()
    {
        SceneManager.LoadScene("Start"); 
    }

    public int[,] modeKiller()
    {
        int[,] groups = new int[9, 9];
        int groupID = 1;
        for (int y = 0; y < 9; y++)
        {
            for (int x = 0; x < 9; x++)
            {
                int id = y * 9 + x;
                if(groups[x, y] == 0)
                {
                    groups[x, y] = groupID;
                    int xx = x, yy = y;
                    int groupMemebers = 1;
                    bool goFurther = true;
                    for (int z = 0; z < 9; z++)
                    {
                        int face = UnityEngine.Random.Range(0,4);
                        //right down left up
                        int destX = xx + (face == 0 ? 1 :
                                        face == 2 ? -1 : 0);
                        int destY = yy + (face == 1 ? 1 :
                                        face == 3 ? -1 : 0);
                        if (destX == -1 || destX == 9 || destY == -1 || destY == 9) continue;
                        int possible = 0;
                        for (int jajebie = 0; jajebie < 4; jajebie++)
                        {
                            int checkX = destX + (jajebie == 0 ? 1 :
                                jajebie == 2 ? -1 : 0);
                            int checkY = destY + (jajebie == 1 ? 1 :
                                jajebie == 3 ? -1 : 0);
                            if (checkX == -1 || checkX == 9 || checkY == -1 || checkY == 9) continue;
                            if (groups[checkX, checkY] == groupID) possible++;
                        }
                        Debug.Log(possible.ToString());
                        if (possible > 1) continue;
                        if (groups[destX, destY] != 0) continue;
                        groups[destX, destY] = groupID;
                        xx = destX; yy = destY;
                        if (UnityEngine.Random.Range(0, groupMemebers) > 0) break;
                        groupMemebers++;
                    }

                    groupID++;
                    
                }
            }

        }
        return groups;
    }
    public Color randomColor() {
        return new Color(
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            UnityEngine.Random.Range(0f, 1f),
            0.5f);
    }


}
