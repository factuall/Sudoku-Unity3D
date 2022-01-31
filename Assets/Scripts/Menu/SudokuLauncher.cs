using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SudokuLauncher : MonoBehaviour
{

    public static int launchDifficulty = 30;

    

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LaunchSudoku(int difficulty)
    {
        launchDifficulty = difficulty;
        SceneManager.LoadScene("Sudoku");
    }

}
