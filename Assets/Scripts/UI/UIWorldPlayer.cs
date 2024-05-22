using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIWorldPlayer : MonoBehaviour
{
    [SerializeField]
    private UIHealth m_playerHealth;

    private PlayerCharacter m_owningCharacter;
    public void Setup(PlayerCharacter ch)
    {
        m_owningCharacter = ch;

        var ps = ch.GetComponent<PlayerState>();

        ps.EOnHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged(float newPercent)
    {
        m_playerHealth.SetHealth(newPercent);
    }
}
