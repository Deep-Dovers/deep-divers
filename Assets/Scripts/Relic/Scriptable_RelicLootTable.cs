using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AvailableRelicList",
    menuName = "Scriptable Objects/Relic/Basic Relic Loot Table Data", order = 1)]
    public class Scriptable_RelicLootTable : ScriptableObject
    {
        public GameObject LootDisplayPrefab;

        [Header("List")]
        public List<RelicLootTableEntry> LootTable = new();

        /// <summary>
        /// Get relic based on spawn rate
        /// </summary>
        /// <returns>Relic</returns>
        public Scriptable_RelicBase GetRandomRelic()
        {


            return null;
        }
    }
}