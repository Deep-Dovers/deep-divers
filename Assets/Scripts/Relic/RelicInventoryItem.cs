using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Relics
{
    /// <summary>
    /// This class is for the one in the inventory
    /// </summary>
    public class RelicInventoryItem
    {
        public Scriptable_RelicBase Data { get; private set; }
        public RelicRarity Rarity { get; private set; }

        public void SetData(Scriptable_RelicBase data)
        {
            Data = data;
        }

        public void SetRarity(RelicRarity rarity)
        {
            Rarity = rarity;
        }
    }
}