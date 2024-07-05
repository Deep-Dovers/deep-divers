using NUnit.Framework;
using Pathfinding;
using Relics;
using System.Collections.Generic;
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

    [Space, Header("Detection Variable")]
    [SerializeField, Tooltip("The Fov Angle")]
    public float fovAngle = 60f;
    [SerializeField, Tooltip("Range of Ai Vision")]
    public float fovRange = 2.6f;
    [SerializeField, Tooltip("Direction of the Fov")]
    public Vector2 direction;

    [SerializeField]
    Transform[] m_patrolPath;

    [SerializeField]
    List<RelicLootDrop> LootList;

    [SerializeField]
    GameObject m_relicPrefab;

    Path m_path;
    Seeker m_seeker;
    Rigidbody2D m_rigidbody;
    int m_currentWaypoint = 0;
    AiActionStateState m_curState;
    AiActionStateState m_prevState;

    private bool hasLineOfSight = false;
    private GameObject m_playerRefs;
    // Start is called before the first frame update
    void Start()
    {
        m_curHP = m_Hp; 
        m_seeker = GetComponent<Seeker>();
        m_rigidbody = GetComponent<Rigidbody2D>();
        m_seeker.StartPath(m_rigidbody.position, m_patrolPath[0].position);
    }
    void OnPathComplete(Path p)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        //if()
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
    public virtual void Kill()
    {
        m_textMesh.color = Color.red;
        dropLoot();
        Destroy(gameObject, 2f);
    }
    public void UpdateAiState()
    {

    }
    public void dropLoot()
    {
        m_textMesh.text = "Dropping Loot";
        //LootTableRefs.GetRandomRelics(ref LootList);
        GameObject SpawnedRelic = Instantiate(m_relicPrefab);
        //Need add the logic that determind what spawned
        SpawnedRelic.GetComponent<Relic>().SetSpawnData(LootList[0].DropRarity, LootList[0].Relic);
    }
}
