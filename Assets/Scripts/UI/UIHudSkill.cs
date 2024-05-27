using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        private AbilityModifierBase m_abilityModInstRef;

        [Header("Keypress")]
        [SerializeField]
        private TextMeshProUGUI m_keyTxt;
        [SerializeField]
        private string m_keyPressText;

        private void OnValidate()
        {
            if (m_keyTxt)
            {
                m_keyTxt.transform.parent.gameObject.SetActive(m_isActiveType);
                m_keyTxt.text = m_keyPressText;
            }
        }

        public void ShowAbility(bool show)
        {
            gameObject.SetActive(show);
        }

        public void SetAbility(AbilityInstanceBase a)
        {
            m_abilityInstRef = a;
            m_ability = a.AbilityData;

            m_relicIcon.sprite = m_ability.UIIcon;

            if(a.InitialCooldownTime <= 0f)
            {
                m_relicIcon.color = Color.white;
                SetCooldown(a.CurrentCooldown / a.CooldownTime);
            }
            else
            {
                m_relicIcon.color = Color.grey;
                SetCooldown(a.InitialCooldownTime / a.CooldownTime);
            }

            a.EOnAbilityCooldownUpdate.AddListener(SetCooldown);
            a.EOnAbilityTriggered.AddListener(OnAbilityTriggered);
        }

        public void SetModifier(AbilityModifierBase a)
        {
            m_abilityModInstRef = a;
            
            m_relicIcon.sprite = a.Icon;
            m_relicIcon.color = Color.white;
            SetCooldown(0f);
        }

        public void RemoveAbility(AbilityInstanceBase a)
        {
            m_abilityInstRef.EOnAbilityCooldownUpdate.RemoveListener(SetCooldown);
            m_abilityInstRef.EOnAbilityTriggered.RemoveListener(OnAbilityTriggered);

            //set to null just so we can see
            m_relicIcon.sprite = null;
            m_relicIcon.color = Color.magenta;

            m_abilityInstRef = null;
            m_ability = null;

            SetCooldown(0f);
        }

        public void RemoveModifier(AbilityModifierBase a)
        {
            m_relicIcon.sprite = null;
            m_relicIcon.color = Color.magenta;

            m_abilityModInstRef = null;
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