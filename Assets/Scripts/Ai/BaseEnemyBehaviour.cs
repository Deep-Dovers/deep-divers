using NUnit.Framework;
using UnityEngine;

public class BaseEnemyBehaviour : MonoBehaviour
{

    public enum AiActionStateState
    {
        IDLE = 0,
        PATROL,
        CHASE,
        ATTACKING,
        DEAD,
    };

    [SerializeField, Tooltip("HP of the Ai")]
    private float m_Hp = 100f;
    private float m_curHP;
    //! Temp solution to show enemy hp
    public TextMesh m_textMesh;

    public float DetectionRange;
    private bool hasLineOfSight = false;
    // Start is called before the first frame update
    void Start()
    {
        m_curHP = m_Hp; 
    }
    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        
    }
    public void DealDamage(float dmg)
    {
        m_curHP -= dmg;
        m_textMesh.text = "HP: "+ m_curHP +"/100";
        if (m_curHP <= 0)
        {
            //! Kill the Ai
            Kill();
        }
    }
    public void Kill()
    {
        m_textMesh.color = Color.red;
        m_textMesh.text = "I think i died";
        Destroy(gameObject, 2f);
    }


}
