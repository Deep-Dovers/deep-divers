using NaughtyAttributes;
using Relics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRelicInventoryItem : MonoBehaviour
{
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
    }

    private void Awake()
    {
        if(m_hoverFrame)
            m_hoverFrame.gameObject.SetActive(false);
    }

    public void Initialize(Relic relic)
    {
        m_relicData = relic.Data;
        m_currentRarity = relic.RelicRarity;

        m_relicIconImg.sprite = relic.Data.DefaultIcon;
        m_relicDesc = relic.Data.DefaultDescription;
    }

    public void OnSelection(bool isSelected)
    {
        m_hoverFrame.gameObject.SetActive(isSelected);
    }
}
