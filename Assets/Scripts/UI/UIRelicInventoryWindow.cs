using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRelicInventoryWindow : UIWindow
{
    [Header("Prefab")]
    [SerializeField]
    private UIRelicInventoryItem m_itemPrefab;

    [Header("Controls")]
    [SerializeField]
    private Button m_closeBtn;

    [Header("Section - Inventory")]
    [SerializeField]
    private ScrollRect m_inventoryScroll;
    private Transform m_scrollContent => m_inventoryScroll.content;

    [Header("Section - Overview")]
    [SerializeField]
    private Transform m_overview;

    [Header("Contents")]
    public List<UIRelicInventoryItem> Relics = new();
    

}
