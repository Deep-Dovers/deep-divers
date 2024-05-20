using UI;
using UnityEngine;
using TMPro;

public class UIGameplay : MonoBehaviour
{
    [SerializeField]
    private AbilityList m_abilities;

    [Header("Player Number")]
    [SerializeField]
    private TextMeshProUGUI m_playerNumTxt;

    [Header("Health")]
    [SerializeField]
    private UIHealth m_hp;

    [Header("Skill/Abilities")]
    [SerializeField]
    private UIHudSkill[] m_activeSkills;
    [SerializeField]
    private UIHudSkill[] m_passiveSkills;
    public UIHudSkill TestBasic;

    private void Awake()
    {
        for (int i = 0; i < m_activeSkills.Length; i++)
            m_activeSkills[i].gameObject.SetActive(false);
        for (int i = 0; i < m_passiveSkills.Length; i++)
            m_passiveSkills[i].gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetAbilityListReference(AbilityList abilityList)
    { 
        m_abilities = abilityList;

        m_abilities.EOnAbilityEquipped.AddListener(AbilityEquipped);

        //loop through current list to check what has been added
        for (int i = 0; i < m_abilities.AbilityInstances.Count; i++)
            AbilityEquipped(m_abilities.AbilityInstances[i], i, true);
    }

    void AbilityEquipped(AbilityInstanceBase a, int i, bool eq)
    {
        //ignore basic attack
        if (i <= 0)
        {
            if (eq)
                TestBasic.SetAbility(a);
            else
                TestBasic.RemoveAbility(a);

            return;
        }

        if(a.Type == Relics.RelicSkillTypes.Active)
        {
            m_activeSkills[i].gameObject.SetActive(eq);

            if(eq)
                m_activeSkills[i].SetAbility(a);
            else
                m_activeSkills[i].RemoveAbility(a);
        }
        else
        {
            m_passiveSkills[i].gameObject.SetActive(eq);

            if (eq)
                m_passiveSkills[i].SetAbility(a);
            else
                m_passiveSkills[i].RemoveAbility(a);
        }
    }
}
