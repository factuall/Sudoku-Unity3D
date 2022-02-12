using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuRandom : MonoBehaviour
{
    public SudokuLauncher launcher;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startRandomplay()
    {
        launcher.LaunchSudoku(Mathf.RoundToInt(Random.Range(10,70)));
    }
}
