using AssetUsageDetectorNamespace;
using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Attach this class to any player or enemy to assign abilities to them!
/// </summary>
public class AbilityList : MonoBehaviour
{
    [SerializeField]
    private AbilityData m_basicAttackData;
    private AbilityInstanceBase m_basicAttackAbilityInst;

    [Header("Un/Equippable")]
    public List<AbilityData> Abilities = new();
    public List<AbilityInstanceBase> AbilityInstances
    { get; protected set; } = new();

    [Header("Modifiers")]
    public List<AbilityModifierBase> Mods = new();

    [SerializeField, ReadOnly]
    private List<float> m_abilityCd = new();
    //this is honestly just for easy reference, might delete
    private Dictionary<AbilityInstanceBase, float> m_abilityToCd = new();
    private Coroutine m_cdCr = null;

    //instance, index, equip/unequip
    public UnityEvent<AbilityInstanceBase, int, bool> EOnAbilityEquipped = new();
    public UnityEvent<AbilityModifierBase, int, bool> EOnModifierEquipped = new();

    //temp public cos idk what will be common between player char and enemy
    [HideInInspector]
    public Vector2 MyFacing = Vector2.zero;

    private void Awake()
    {
        m_basicAttackAbilityInst = Equip(m_basicAttackData);
    }

    private void OnDestroy()
    {
        foreach (var inst in AbilityInstances)
        {
            inst.EOnAbilityTriggered.RemoveAllListeners();
            inst.EOnAbilityCooldownUpdate.RemoveAllListeners();
        }

        EOnAbilityEquipped.RemoveAllListeners();

        m_abilityCd.Clear();
        m_abilityToCd.Clear();
    }

    public AbilityInstanceBase Equip(AbilityData data)
    {
        AbilityInstanceBase inst = AbilityInstances.Find(x => x.AbilityData == data);

        if (inst != null)
        {
            return null;
        }
        
        var skill = data.CreateAbilityInstance();
        Equip(skill);

        return skill;
    }

    public void Equip(AbilityInstanceBase ability)
    {
        if (m_abilityToCd.ContainsKey(ability))
        {
            Debug.Log("Ability already exists, ignoring");
            return;
        }

        //data
        Abilities.Add(ability.AbilityData);

        //instance
        AbilityInstances.Add(ability);

        m_abilityCd.Add(ability.AbilityData.InitialCooldownTime);
        m_abilityToCd.Add(ability, ability.AbilityData.InitialCooldownTime);

        EOnAbilityEquipped?.Invoke(ability, AbilityInstances.Count - 1, true);
    }

    public void EquipModifier(AbilityModifierBase mod)
    {
        var m = Mods.Find(x => x == mod);

        if (!Mods.Exists(x => x == mod))
        {
            Mods.Add(mod);
            EOnModifierEquipped?.Invoke(mod, Mods.Count - 1, true);
        }
    }

    public void UnEquip(AbilityData data)
    {
        AbilityInstanceBase inst = AbilityInstances.Find(x => x.AbilityData == data);

        if (inst == null)
        {
            return;
        }

        UnEquip(inst);
    }

    public void UnEquip(AbilityInstanceBase ability)
    {
        if (!m_abilityToCd.ContainsKey(ability))
        {
            Debug.Log("Ability does not exist, ignoring");
            return;
        }

        int ind = AbilityInstances.IndexOf(ability);
        EOnAbilityEquipped?.Invoke(ability, ind, false);

        //data
        Abilities.Remove(ability.AbilityData);

        AbilityInstances.Remove(ability);

        m_abilityCd.RemoveAt(ind);
        m_abilityToCd.Remove(ability);
    }

    public void Execute(AbilityInstanceBase ability)
    {
        int index = 0;

        index = AbilityInstances.IndexOf(ability);

        Execute(index);
    }

    /// <summary>
    /// Execute by index. 0 for basic
    /// </summary>
    /// <param name="ind">index of ability list</param>
    public void Execute(int ind = 0)
    {
        if (!CanExecute(ind))
        {
            Debug.Log("Tried to execute but ability on cooldown!");
            return;
        }
        if (AbilityInstances.Count > ind)
        {
            AbilityInstances[ind].Execute(transform.position, MyFacing);
            InitiateCooldown(ind);
        }
        else
            Debug.Log($"{name} Tried to execute {ind} ability but does not exist");
    }

    public bool CanExecute(int ind = 0)
    {
        return GetCurrentCooldown(ind) <= 0f;
    }

    private void InitiateCooldown(int ind = -1)
    {
        float cd = 0f;

        if (ind < AbilityInstances.Count)
        {
            cd = AbilityInstances[ind].CooldownTime;
            m_abilityToCd[AbilityInstances[ind]] = cd;
            m_abilityCd[ind] = cd;
        }

        if (m_cdCr == null)
        {
            m_cdCr = StartCoroutine(Cooldown());
        }
    }

    private float GetCurrentCooldown(int ind = 0)
    {
        if (ind < m_abilityToCd.Count)
            return m_abilityToCd[AbilityInstances[ind]];
        else
            return 0f;
    }

    /// <summary>
    /// Tick for all ability cooldown
    /// </summary>
    /// <returns></returns>
    protected virtual IEnumerator Cooldown()
    {
        //not using direct yield because can use tick to update ui
        while (true)
        {
            bool canStopCrFlag = true;

            for (int i = 0; i < m_abilityCd.Count; i++)
            {
                if (m_abilityCd[i] > 0f)
                {
                    m_abilityCd[i] -= Time.deltaTime;
                    m_abilityToCd[AbilityInstances[i]] = m_abilityCd[i];
                    AbilityInstances[i].SetCooldown(m_abilityCd[i]);

                    canStopCrFlag = false;

                    //if first instance of hitting 0
                    if (m_abilityCd[i] <= 0f)
                    {
                        AbilityInstances[i].EOnAbilityTriggered?.Invoke(false);

                        m_abilityCd[i] = 0f;
                        m_abilityToCd[AbilityInstances[i]] = m_abilityCd[i];
                        AbilityInstances[i].SetCooldown(m_abilityCd[i]);
                    }
                }
            }

            //update UI here
            //UpdateUI();

            if (canStopCrFlag)
                break;

            yield return null;
        }

        m_cdCr = null;

        yield return null;
    }
}
