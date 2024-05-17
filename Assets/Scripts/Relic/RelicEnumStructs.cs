using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Relics
{
    /// <summary>
    /// GDD: https://docs.google.com/document/d/19FPeAx0NTitVsxAbg79FQmvzDeT6wDa0VmTIZsrgZPY/
    /// </summary>
    public enum RelicTypes 
    { 
        Active = 0,      //where skill gives players new abilities or modifies basic attacks
        Passive,        //augment/enhance abilities
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
    public struct RelicStatPerRarity
    {
        public RelicRarity Rarity;
        [ResizableTextArea, Tooltip("If left blank will use basic description")]
        public string Description;
        [HideInInspector]
        public Sprite UpgradedIcon;
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
        [AllowNesting, Expandable]
        public Scriptable_RelicBase Relic;
        [Range(0f, 100f), Tooltip("The drop rate of THIS relic from 0 to 100%. Rarity drop rate adjusted inside.")]
        public float AppearanceRate;
        public List<RarityToSpawnRate> RarityToSpawnRate;

        public (bool spawn, RelicRarity spawnRarity) GetSpawn(float rng)
        {
            int last = RarityToSpawnRate.Count - 1;
            RelicRarity r = RarityToSpawnRate[last].Rarity;

            for(int i = last; i >= 0; i--)
            {
                if(rng < RarityToSpawnRate[i].Spawn.DropRate)
                {
                    return (true, RarityToSpawnRate[i].Rarity);
                }
            }

            return (false, Relic.BaseRarity);
        }
    }

    [System.Serializable]
    public struct RelicLootDrop
    {
        public Scriptable_RelicBase Relic;
        public RelicRarity DropRarity;

        public RelicLootDrop(Scriptable_RelicBase r, RelicRarity rare)
        {
            Relic = r;
            DropRarity = rare;
        }
    }

    [System.Serializable]
    public struct RarityToSpawnRate
    {
        public RelicRarity Rarity;
        public RelicSpawn Spawn;
    }
}