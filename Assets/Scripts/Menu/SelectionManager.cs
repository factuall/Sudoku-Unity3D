using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GridLayoutGroup menusGrid;
    public MenuPage[] menuPages;
    public Canvas mainCanvas;
    int page = 0;

    // Start is called before the first frame update
    void Start()
    {
        menusGrid = GetComponent<GridLayoutGroup>();
        menuPages = GetComponentsInChildren<MenuPage>();
    }

    // Update is called once per frame
    void Update()
    {
        float newX = ((page * Screen.width) - transform.position.x) * Time.deltaTime;
        transform.position = new Vector3(mainCanvas.transform.position.x + newX, mainCanvas.transform.position.y, 0);
    }
}
