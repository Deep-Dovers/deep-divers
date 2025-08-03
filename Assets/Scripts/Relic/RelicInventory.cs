using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInventory : MonoBehaviour
{
    [SerializeField]
    public List<RelicInventoryItem> m_relicsOwned = new();
    public List<RelicInventoryItem> RelicsOwned => m_relicsOwned;
    [SerializeField, ReadOnly]
    public int Count = 0; //debug

    [Header("UI")]
    [SerializeField]
    private UIRelicInventoryWindow m_windowPrefab;
    private UIRelicInventoryWindow m_window;

    void Awake()
    {
        if (!m_window)
            m_window = Instantiate(m_windowPrefab, Vector3.zero, Quaternion.identity);

        m_window.Close();
    }

    public void ToggleWindow()
    {
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

        Count = m_relicsOwned.Count;
    }

    public void RemoveRelic(Relic relic)
    {
        RelicInventoryItem r = DoesRelicExist(relic);

        if (r != null)
        {
            m_relicsOwned.Remove(r);

            m_window?.UpdateRelicList(RelicsOwned);
        }

        Count = m_relicsOwned.Count;
    }

    public void UpgradeRelic(int id)
    {

    }

    public void UpdateRelic(Relic relic)
    {
    }
}
