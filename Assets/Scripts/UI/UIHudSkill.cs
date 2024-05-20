using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIHudSkill : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private Image m_relicIcon;
        [SerializeField]
        private Image m_cdOverlay;
        [SerializeField]
        private bool m_isActiveType = false;

        [Header("Debug")]
        [SerializeField, ReadOnly, Range(0f, 1f)]
        private float m_currCdPercent = 0f;
        [SerializeField, ReadOnly, Expandable]
        private AbilityData m_ability;
        private AbilityInstanceBase m_abilityInstRef;

        public void ShowAbility(bool show)
        {
            gameObject.SetActive(show);
        }

        public void SetAbility(AbilityInstanceBase a)
        {
            m_abilityInstRef = a;
            m_ability = a.AbilityData;

            m_relicIcon.sprite = m_ability.UIIcon;

            m_currCdPercent = 0f;

            a.EOnAbilityCooldownUpdate.AddListener(SetCooldown);
            a.EOnAbilityTriggered.AddListener(OnAbilityTriggered);
        }

        public void RemoveAbility(AbilityInstanceBase a)
        {
            m_abilityInstRef.EOnAbilityCooldownUpdate.RemoveListener(SetCooldown);
            m_abilityInstRef.EOnAbilityTriggered.RemoveListener(OnAbilityTriggered);

            m_abilityInstRef = null;
            m_ability = null;

            m_currCdPercent = 0f;
        }

        public void SetCooldown(float cooldownPercent)
        {
            m_currCdPercent = cooldownPercent;
            m_cdOverlay.fillAmount = cooldownPercent;
        }

        private void OnAbilityTriggered(bool active)
        {
            if(active)
            {
                m_relicIcon.color = Color.grey;
                SetCooldown(1f);
            }
            else
            {
                m_relicIcon.color = Color.white;
                SetCooldown(0f);
            }
        }
    }
}