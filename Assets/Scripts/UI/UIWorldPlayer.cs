using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class UIWorldPlayer : MonoBehaviour
{
    [SerializeField]
    private UIHealth m_playerHealth;

    private PlayerCharacter m_owningCharacter;
    public void Setup(PlayerCharacter ch)
    {
        m_owningCharacter = ch;

        //constraints
        ParentConstraint pConstraint = GetComponent<ParentConstraint>();
        pConstraint.AddSource(
            new ConstraintSource() {
                sourceTransform = m_owningCharacter.transform,
                weight = 1f
            });
        pConstraint.SetTranslationOffset(0, new Vector3(0, 2.05f, 0));

        var ps = ch.GetComponent<PlayerState>();

        ps.EOnHealthChanged.AddListener(OnHealthChanged);
    }

    void OnHealthChanged(float newPercent)
    {
        m_playerHealth.SetHealth(newPercent);
    }
}
