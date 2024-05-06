using System;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    /// <summary>
    /// GDD: https://docs.google.com/document/d/19FPeAx0NTitVsxAbg79FQmvzDeT6wDa0VmTIZsrgZPY/
    /// </summary>
    public enum RelicTypes 
    { 
        Skill = 0,      //where skill gives players new abilities or modifies basic attacks
        Support,        //augment/enhance abilities
    }

    public enum RelicSkillTypes
    {
        Passive = 0,
        Active = 1,
    }

    public enum RelicRarity
    {
                          //In gacha terms...
        Normal = 0,       //N
        Rare,             //R
        SuperRare,        //SR
        SuperSuperRare,   //SSR
        Epic,             //UR
        Legendary         //UUR?
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

    [System.Serializable]
    public struct RelicStatPerLevel
    {
        public int Level;
        [Multiline, Tooltip("If left blank will use basic description")]
        public string Description;
        public Image UpgradedIcon;
    }

    [System.Serializable]
    public struct RelicSpawn
    {
        [Range(0f, 100f), Tooltip("0 to 100%")]
        public float DropRate;
        public RelicAvailableIn SpawnCondition;
    }

    [System.Serializable]
    public struct RelicLootTableEntry
    {
        public Scriptable_RelicBase Relic;
        public RelicSpawn SpawnSettings;
    }
}