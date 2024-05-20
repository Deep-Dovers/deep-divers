using AssetUsageDetectorNamespace;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private List<AbilityInstanceBase> m_abilityInsts = new();

    [SerializeField, ReadOnly]
    private List<float> m_abilityCd = new();
    //this is honestly just for easy reference, might delete
    private Dictionary<AbilityInstanceBase, float> m_abilityToCd = new();
    private Coroutine m_cdCr = null;

    //testing ui
    public UIGameplay UIGameplay;

    //temp public cos idk what will be common between player char and enemy
    [HideInInspector]
    public Vector2 MyFacing = Vector2.zero;

    private void Awake()
    {
        m_basicAttackAbilityInst = Equip(m_basicAttackData);
    }

    private void OnDestroy()
    {
        foreach (var inst in m_abilityInsts)
        {
            inst.EOnAbilityTriggered.RemoveAllListeners();
            inst.EOnAbilityCooldownUpdate.RemoveAllListeners();
        }

        m_abilityCd.Clear();
        m_abilityToCd.Clear();
    }

    public AbilityInstanceBase Equip(AbilityData data)
    {
        AbilityInstanceBase inst = m_abilityInsts.Find(x => x.AbilityData == data);

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
        m_abilityInsts.Add(ability);

        m_abilityCd.Add(ability.AbilityData.InitialCooldownTime);
        m_abilityToCd.Add(ability, ability.AbilityData.InitialCooldownTime);

        //testing
        if(UIGameplay)
        {
            ability.RegisterUI(UIGameplay.TestBasic);
        }
    }

    public void UnEquip(AbilityData data)
    {
        AbilityInstanceBase inst = m_abilityInsts.Find(x => x.AbilityData == data);

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

        int ind = m_abilityInsts.IndexOf(ability);

        //data
        Abilities.Remove(ability.AbilityData);

        m_abilityInsts.Remove(ability);

        m_abilityCd.RemoveAt(ind);
        m_abilityToCd.Remove(ability);
    }

    public void Execute(AbilityInstanceBase ability)
    {
        int index = 0;

        index = m_abilityInsts.IndexOf(ability);

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
        if (m_abilityInsts.Count > ind)
        {
            m_abilityInsts[ind].Execute(transform.position, MyFacing);
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

        if (ind < m_abilityInsts.Count)
        {
            cd = m_abilityInsts[ind].CooldownTime;
            m_abilityToCd[m_abilityInsts[ind]] = cd;
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
            return m_abilityToCd[m_abilityInsts[ind]];
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
                    m_abilityToCd[m_abilityInsts[i]] = m_abilityCd[i];
                    m_abilityInsts[i].SetCooldown(m_abilityCd[i]);

                    canStopCrFlag = false;

                    //if first instance of hitting 0
                    if (m_abilityCd[i] <= 0f)
                        m_abilityInsts[i].EOnAbilityTriggered?.Invoke(false);
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
