using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPage : MonoBehaviour
{
    public string menuTitle;
    public Text titleDisplayText;


    // Start is called before the first frame update
    void Start()
    {
        titleDisplayText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        titleDisplayText.text = menuTitle;
    }
}
