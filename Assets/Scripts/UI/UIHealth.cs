using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private Slider m_baseSlider;
    [SerializeField]
    private TextMeshProUGUI m_hpTxt;

    private float m_percent;

    public void SetHealth(float percent)
    {
        m_percent = percent;
        m_baseSlider.value = m_percent;
        m_hpTxt.text = Mathf.CeilToInt(m_percent).ToString() + "%";
    }

    public void SetHealth(float curr, float max)
    {
        SetHealth(curr / max);
    }
}
