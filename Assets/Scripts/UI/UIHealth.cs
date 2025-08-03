using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHealth : MonoBehaviour
{
    [SerializeField]
    private Slider m_baseSlider;
    [SerializeField]
    private TextMeshProUGUI m_hpTxt;

    [SerializeField, Range(0f, 1f)]
    private float m_percent;

    private void OnValidate()
    {
         SetHealth(m_percent);
    }

    public void SetHealth(float percent)
    {
        m_percent = percent;
        m_baseSlider.value = m_percent;

        if(m_hpTxt != null)
            m_hpTxt.text = Mathf.CeilToInt(m_percent * 100f).ToString() + "%";
    }

    public void SetHealth(float curr, float max)
    {
        SetHealth(curr / max);
    }
}
