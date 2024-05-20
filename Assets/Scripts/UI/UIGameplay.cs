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

    // Update is called once per frame
    void Update()
    {
        //!! very first iteration to fix later
        if (m_abilities == null)
            return;

        for(int i = 0; i < m_activeSkills.Length; i++)
        {
            m_activeSkills[i].gameObject.SetActive(i < m_abilities.Abilities.Count);
        }
    }
}
