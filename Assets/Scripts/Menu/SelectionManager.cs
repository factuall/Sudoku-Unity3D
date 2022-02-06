using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectionManager : MonoBehaviour
{
    public GridLayoutGroup menusGrid;
    public MenuPage[] menuPages;
    public Canvas mainCanvas;
    public RectTransform menuRect;
    public RectTransform mainRect;
    public int page = 0;

    // Start is called before the first frame update
    void Start()
    {
        menuRect = GetComponent<RectTransform>();
        mainRect = mainCanvas.GetComponent<RectTransform>();
        menusGrid = GetComponent<GridLayoutGroup>();
        menuPages = GetComponentsInChildren<MenuPage>();
    }

    // Update is called once per frame
    void Update()
    {
        float newX = (menuRect.anchoredPosition.x - ((page * -1) * menuRect.rect.width)) * Time.deltaTime * 8;
        menuRect.anchoredPosition = new Vector2(menuRect.anchoredPosition.x + (0 - newX), 0);
    }
}
