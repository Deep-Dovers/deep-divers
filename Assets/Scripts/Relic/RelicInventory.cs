using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInventory : MonoBehaviour
{
    public List<RelicInventoryItem> m_relicsOwned = new();
    public List<RelicInventoryItem> RelicsOwned => m_relicsOwned;

    [Header("UI")]
    [SerializeField]
    private UIRelicInventoryWindow m_windowPrefab;
    private UIRelicInventoryWindow m_window;

    void Awake()
    {
        
    }

    public void ToggleWindow()
    {
        if (!m_window)
            m_window = Instantiate(m_windowPrefab, Vector3.zero, Quaternion.identity);

        m_window.ToggleWindow();
    }

    private RelicInventoryItem DoesRelicExist(Relic r)
    {
        return m_relicsOwned.Find(x => x.Data == r.Data);
    }

    public void AddRelic(Relic relic)
    {
        if (DoesRelicExist(relic) == null)
        {
            RelicInventoryItem newItem = new RelicInventoryItem();
            newItem.SetData(relic.Data);
            newItem.SetRarity(relic.RelicRarity);

            m_relicsOwned.Add(newItem);

            m_window?.UpdateRelicList(RelicsOwned);
        }
    }

    public void RemoveRelic(Relic relic)
    {
        RelicInventoryItem r = DoesRelicExist(relic);

        if (r != null)
        {
            m_relicsOwned.Remove(r);

            m_window?.UpdateRelicList(RelicsOwned);
        }
    }

    public void UpgradeRelic(int id)
    {

    }

    public void UpdateRelic(Relic relic)
    {
    }
}
