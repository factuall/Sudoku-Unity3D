using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SudokuLauncher : MonoBehaviour
{

    public static int launchDifficulty = 30;

    public Slider difficultySlider;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (launchDifficulty != difficultySlider.value) launchDifficulty = ((int)difficultySlider.value);
    }

    public void LaunchSudoku()
    {
        SceneManager.LoadScene("Sudoku");
    }

}
