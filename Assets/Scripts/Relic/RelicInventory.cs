using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInventory : MonoBehaviour
{
    [SerializeField]
    private List<Relic> m_relicsOwned = new List<Relic>();
    public List<Relic> RelicsOwned => m_relicsOwned;

    [Header("UI")]
    [SerializeField]
    private UIRelicInventoryWindow m_window;

    void Awake()
    {
        
    }

    private Relic DoesRelicExist(Relic r)
    {
        return m_relicsOwned.Find(x => x == r);
    }

    public void AddRelic(Relic relic)
    {
        if(!DoesRelicExist(relic))
            m_relicsOwned.Add(relic);
    }

    public void RemoveRelic(Relic relic)
    {
        Relic r = DoesRelicExist(relic);

        if (r)
            m_relicsOwned.Remove(relic);
    }

    public void UpgradeRelic(int id)
    {

    }

    public void UpdateRelic(Relic relic)
    {
    }
}
