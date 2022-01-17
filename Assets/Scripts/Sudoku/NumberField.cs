using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class NumberField : MonoBehaviour
{

    public int content = 0;
    public int id = 0;
    public SudokuManager Manager;
    public Text FieldText;
    public Image FieldImage;
    public bool highlighted;

    // Start is called before the first frame update
    void Start()
    {
        FieldText = this.GetComponentInChildren<Text>();
        FieldImage = this.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (content != 0)
            FieldText.text = content.ToString();
        else
            FieldText.text = "";
    }

    public void FieldPressed()
    {
        Manager.FieldPressed(id);
    }
}
