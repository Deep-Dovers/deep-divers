using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Relics
{
    public class RelicSystem : MonoBehaviour
    {
        [SerializeField]
        private Scriptable_RelicLootTable RelicLootTable;

        private void OnValidate()
        {

        }

        private void Awake()
        {

        }

        private void OnDestroy()
        {

        }

        public void HotSwapRelicList(Scriptable_RelicLootTable list)
        {
            if(list != RelicLootTable)
            {
                RelicLootTable = list;
            }
        }

        [Button]
        public void GenerateRelics()
        {
            List<RelicLootDrop> loot = new();
            RelicLootTable.GetRandomRelics(ref loot);

            for (int i = 0; i < loot.Count; i++)
            {
                Relic r = Instantiate(RelicLootTable.LootGamePrefab, Vector3.zero, Quaternion.identity);
                r.SetSpawnData(loot[i].DropRarity, loot[i].Relic);
            }
        }
    }
}