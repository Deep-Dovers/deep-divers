using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Relay;
using UnityEngine;
using UnityEngine.UI;

public class UIRelicInventoryItem : MonoBehaviour
{
    private Animator m_animator;

    [Header("Controls")]
    [SerializeField]
    private Image m_hoverFrame;

    [Header("Contents")]
    [SerializeField]
    private Image m_relicIconImg;
    [SerializeField]
    private string m_relicDesc;

    [Header("Data")]
    [SerializeField, ReadOnly]
    private Scriptable_RelicBase m_relicData;
    [SerializeField, ReadOnly]
    private RelicRarity m_currentRarity;

    private void OnValidate()
    {
        if(!m_hoverFrame)
            m_hoverFrame = transform.Find("HoverFrame").GetComponent<Image>();

        if(!m_relicIconImg)
            m_relicIconImg = transform.Find("RelicFrame/RelicIcon").GetComponent<Image>();

        m_animator = GetComponent<Animator>();
    }

    private void Awake()
    {
        m_animator = GetComponent<Animator>();

        //if(m_hoverFrame)
        //    m_hoverFrame.gameObject.SetActive(false);

        GetComponent<Button>().onClick.AddListener(OnClick);

        m_relicIconImg.sprite = m_relicData?.DefaultIcon;
        m_relicDesc = m_relicData?.DefaultDescription;
    }

    private void OnDestroy()
    {
        GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void Initialize(RelicInventoryItem relic)
    {
        m_relicData = relic.Data;
        m_currentRarity = relic.Rarity;

        if (!m_relicIconImg)
            m_relicIconImg = transform.Find("RelicFrame/RelicIcon").GetComponent<Image>();

        m_relicIconImg.sprite = relic.Data.DefaultIcon;
        m_relicDesc = relic.Data.DefaultDescription;
    }

    public void OnSelection(bool isSelected)
    {
        m_hoverFrame.gameObject.SetActive(isSelected);

        m_animator.Play(isSelected ? "Highlighted" : "Normal");
    }

    public void OnClick()
    {
        //m_animator.Play("Pressed");
    }
}
