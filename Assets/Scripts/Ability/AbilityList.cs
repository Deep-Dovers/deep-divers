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

    private void Awake()
    {
        m_basicAttackAbilityInst = new BasicAttack(m_basicAttackData);

        //basic
        m_abilityCd.Add(m_basicAttackAbilityInst.InitialCooldownTime);
        m_abilityToCd.Add(m_basicAttackAbilityInst, m_basicAttackAbilityInst.InitialCooldownTime);

        //start from 1 bc 0 is basic atk
        int i = 1;
        foreach(var ability in m_abilityInsts)
        {
            m_abilityCd.Add(ability.InitialCooldownTime);
            m_abilityToCd.Add(ability, ability.InitialCooldownTime);

            if (ability.InitialCooldownTime > 0f)
                InitiateCooldown(i);

            i++;
        }
    }

    private void OnDestroy()
    {
        m_abilityCd.Clear();
        m_abilityToCd.Clear();
    }

    public void Equip(AbilityInstanceBase ability)
    {
        m_abilityInsts.Add(ability);
    }

    public void UnEquip(AbilityInstanceBase ability)
    {
        m_abilityInsts.Remove(ability);
    }

    public void Execute(AbilityInstanceBase ability)
    {
        int index = -1;

        if(ability != m_basicAttackAbilityInst)
            index = m_abilityInsts.IndexOf(ability);

        Execute(index);
    }

    /// <summary>
    /// Execute by index. -1 for basic
    /// </summary>
    /// <param name="ind">index of ability list</param>
    public void Execute(int ind = -1)
    {
        if(!CanExecute(ind))
        {
            Debug.Log("Tried to execute but ability on cooldown!");
            return;
        }

        if (ind < 0)
        {
            m_basicAttackAbilityInst.Execute();
            InitiateCooldown();
        }
        else
        {
            if (m_abilityInsts.Count > ind)
            {
                m_abilityInsts[ind].Execute();
                InitiateCooldown(ind);
            }
            else
                Debug.Log($"{name} Tried to execute {ind} ability but does not exist");
        }
    }

    public bool CanExecute(int ind = -1)
    {
        return GetCurrentCooldown(ind) <= 0f;
    }

    private void InitiateCooldown(int ind = -1)
    {
        float cd = 0f;

        if (ind < 0)
        {
            cd = m_basicAttackAbilityInst.CooldownTime;
            m_abilityToCd[m_basicAttackAbilityInst] = cd;
            m_abilityCd[0] = cd;
        }
        else if (ind < m_abilityInsts.Count)
        {
            cd = m_abilityInsts[ind].CooldownTime;
            m_abilityToCd[m_abilityInsts[ind]] = cd;
            m_abilityCd[ind] = cd;
        }
        
        if(m_cdCr == null)
        {
            m_cdCr = StartCoroutine(Cooldown());
        }
    }

    private float GetCurrentCooldown(int ind = -1)
    {
        if (ind < 0)
            return m_abilityToCd[m_basicAttackAbilityInst];
        else
            return m_abilityToCd[m_abilityInsts[ind]];
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

            for(int i = 0; i < m_abilityCd.Count; i++)
            {
                if (m_abilityCd[i] > 0f)
                {
                    m_abilityCd[i] -= 1f;
                    canStopCrFlag = false;

                    if (i == 0)
                        m_abilityToCd[m_basicAttackAbilityInst] = m_abilityCd[i];
                    else
                        m_abilityToCd[m_abilityInsts[i - 1]] = m_abilityCd[i - 1];
                }
            }

            //update UI here
            //UpdateUI();

            if (canStopCrFlag)
                break;

            yield return new WaitForSeconds(1f);
        }

        m_cdCr = null;

        yield return null;
    }
}
