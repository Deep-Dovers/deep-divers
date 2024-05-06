using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    public class RelicSystem : MonoBehaviour
    {
        public const int MaxRelicSpawn = 3;
        private List<Scriptable_RelicBase> m_spawnedRelics = new();
        public List<Scriptable_RelicBase> AvailableRelics => m_spawnedRelics;

        [SerializeField]
        private Scriptable_AvailableRelicList RelicLootTable;

        private void OnValidate()
        {

        }

        private void Awake()
        {

        }

        private void OnDestroy()
        {

        }

        public void HotSwapRelicList(Scriptable_AvailableRelicList list)
        {
            if(list != RelicLootTable)
            {
                RelicLootTable = list;
            }
        }

        public void GenerateRelics()
        {
            AvailableRelics.Clear();

            for (int i = 0; i < MaxRelicSpawn; i++)
            {
                AvailableRelics.Add(RelicLootTable.GetRandomRelic());
            }
        }
    }
}