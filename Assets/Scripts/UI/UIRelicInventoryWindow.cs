using Relics;
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

    private void Awake()
    {
        m_closeBtn?.onClick.AddListener(Close);

        //UpdateRelicList(null);
    }

    private void OnDestroy()
    {
        m_closeBtn?.onClick.RemoveListener(Close);
    }

    public void UpdateRelicList(List<RelicInventoryItem> list)
    {
        for (int i = m_scrollContent.childCount - 1; i >= 0; i--)
        {
            Destroy(m_scrollContent.GetChild(i).gameObject);
        }

        m_scrollContent.DetachChildren();
        Relics.Clear();

        if (list != null && list.Count > 0)
        {
            foreach (RelicInventoryItem item in list)
            {
                var entry = Instantiate(m_itemPrefab, m_scrollContent);

                entry.Initialize(item);
            }
        }
    }

    private void UpdateOverviewPage()
    {
    }
}
