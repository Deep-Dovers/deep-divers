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
    private UIHealth m_myHealth;

    [Header("Skill/Abilities")]
    [SerializeField]
    private UIHudSkill[] m_activeSkills;
    [SerializeField]
    private UIHudSkill[] m_passiveSkills;
    public UIHudSkill TestBasic;

    [Header("debug")]
    [SerializeField]
    private PlayerCharacter m_owningCharacter;

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

    public void Setup(PlayerCharacter ch)
    {
        m_owningCharacter = ch;

        var ps = ch.GetComponent<PlayerState>();

        ps.EOnHealthChanged.AddListener(OnHealthChanged);

        SetAbilityListReference(ch.GetComponent<AbilityList>());
    }

    public void SetAbilityListReference(AbilityList abilityList)
    { 
        m_abilities = abilityList;

        m_abilities.EOnAbilityEquipped.AddListener(OnAbilityEquipped);
        m_abilities.EOnModifierEquipped.AddListener(OnModifierEquipped);

        //loop through current list to check what has been added
        for (int i = 0; i < m_abilities.AbilityInstances.Count; i++)
            OnAbilityEquipped(m_abilities.AbilityInstances[i], i, true);
    }

    void OnAbilityEquipped(AbilityInstanceBase a, int i, bool eq)
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

        m_activeSkills[i].ShowAbility(eq);

        if(eq)
            m_activeSkills[i].SetAbility(a);
        else
            m_activeSkills[i].RemoveAbility(a);
    }

    void OnModifierEquipped(AbilityModifierBase a, int i, bool eq)
    {
        m_passiveSkills[i].ShowAbility(eq);

        if (eq)
            m_passiveSkills[i].SetModifier(a);
        else
            m_passiveSkills[i].RemoveModifier(a);
    }

    void OnHealthChanged(float newPercent)
    {
        m_myHealth.SetHealth(newPercent);
    }
}
