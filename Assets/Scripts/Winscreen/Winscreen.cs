using UnityEngine;
using UnityEngine.UI;

public class Winscreen : MonoBehaviour
{
    public Text timeText;
    public Text mistakesText;
    public Text scoreText;
    public Text difficultyText;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float min = Mathf.Floor(SudokuManager.playTime / 60);
        float sec = Mathf.RoundToInt(SudokuManager.playTime % 60);

        timeText.text = "time: " +
            (min > 9 ? min.ToString() : "0" + min.ToString())
            + ":" +
            (sec > 9 ? sec.ToString() : "0" + sec.ToString());
        difficultyText.text = "difficulty: " + SudokuLauncher.launchDifficulty.ToString();
        scoreText.text = "score: " + Mathf.RoundToInt(10000 * (SudokuLauncher.launchDifficulty / SudokuManager.playTime)).ToString();
    }
}
