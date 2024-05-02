using System;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    public enum RelicTypes 
    { 
        Offense = 0,
        Status,
        Defense,
        Debuff
    }

    [System.Serializable]
    public struct RelicStatPerLevel
    {
        public int Level;
        [Multiline, Tooltip("If left blank will use basic description")]
        public string Description;
        public Image UpgradedIcon;
    }

    [Flags]
    public enum RelicAvailableIn
    {
        None = 0,
        BattleLoot = 2,
        Boss = 4,
        Shop = 8,
        Treasure = 16,
        Quests = 32,
    }
}