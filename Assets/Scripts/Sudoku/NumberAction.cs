using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NumberAction : MonoBehaviour
{
    public int content = 0;
    public int id = 0;
    public SudokuManager Manager;
    public Text FieldText;
    public Image FieldImage;

    // Start is called before the first frame update
    void Start()
    {
        FieldText = this.GetComponentInChildren<Text>();
        FieldImage = this.GetComponent<Image>();
        FieldText.text = (id+1).ToString();
    }

    // Update is called once per frame
    void Update()
    {
    }

}
