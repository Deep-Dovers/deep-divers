using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    [CreateAssetMenu(fileName = "AvailableRelicList",
    menuName = "Scriptable Objects/AvailableRelicList Data", order = 1)]
    public class Scriptable_AvailableRelicList : ScriptableObject
    {
        public List<Scriptable_RelicBase> LootTable = new();

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