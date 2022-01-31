using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuFreeplay : MonoBehaviour
{
    public Slider difficultySlider;
    public Text difficultyDisplay;
    public SudokuLauncher launcher;

    int difficultyValue = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        difficultyValue = (int)difficultySlider.value;
        difficultyDisplay.text = "difficulty: " + difficultyValue.ToString();
    }

    public void startFreeplay()
    {
        launcher.LaunchSudoku(difficultyValue);
    }

    
}
