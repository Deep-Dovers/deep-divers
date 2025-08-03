using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AvailableRelicList",
    menuName = "Scriptable Objects/Relic/Basic Relic Loot Table Data", order = 100)]
    public class Scriptable_RelicLootTable : ScriptableObject
    {
        public Relic LootGamePrefab;
        public RectTransform LootDisplayUI;

        [Header("Idk")]
        public int MinLootToSpawn = 0;
        [Tooltip("-1 to use the size of array")]
        [InfoBox("not in use")]
        public int MaxLootToSpawn = -1;

        [Header("List")]
        [InfoBox("Make sure that each relic has the correct rarity spawn rate map!")]
        public List<RelicLootTableEntry> LootTable = new();

        public void GetRandomRelics(ref List<RelicLootDrop> lootList)
        {
            if (MaxLootToSpawn <= 0)
                return;

            float rand = Random.Range(0.01f, 100f);
            int randNumToSpawn = Random.Range(MinLootToSpawn, MaxLootToSpawn + 1);
            //Debug.Log("RNG " + rand);

            List<RelicLootTableEntry> availLoot = new();

            //first layer spawn check - what relics to spawn
            foreach (var loot in LootTable)
            {
                if (rand <= loot.AppearanceRate)
                {
                    availLoot.Add(loot);
                }
            }

            //second layer check - relic rarity to spawn
            foreach(var loot in availLoot)
            {
                float rare = Random.Range(0.01f, 100f);

                //Debug.Log("Checking spawn rare " + loot.Relic.DisplayName);
                //Debug.Log("rare rng: " + rare);

                (bool spawn, RelicRarity r) = loot.GetSpawn(rare);

                if (spawn)
                {
                    lootList.Add(new RelicLootDrop(loot.Relic, r));
                    //Debug.Log("spawning a " + r + " " + loot.Relic.DisplayName);
                }
            }

            if(lootList.Count > MaxLootToSpawn)
            {
                Debug.Log("we fucked up bois, removing something");
            }
        }
    }
}