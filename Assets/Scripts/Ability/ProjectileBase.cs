using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileBase : MonoBehaviour
{
    [SerializeField]
    private float m_speed = 10f;
    [SerializeField]
    private float m_damageValue = 1f;
    [SerializeField]
    private float m_lifeTime = 2f;
    [SerializeField]
    private float m_range = 5f;
    [SerializeField, ReadOnly]
    private Vector3 m_direction;
    private Vector3 m_startingposition;

    [Header("VFX/Impact")]
    [SerializeField]
    private GameObject m_impactPrefab;

    public int MaxPenCount = 1;
    private int m_currPenCount = 0;

    //events
    public UnityEvent EOnSpawn { get; private set; } = new();
    public UnityEvent EOnImpact { get; private set; } = new();

    private void OnDestroy()
    {
        EOnImpact.RemoveAllListeners();
    }

    // Update is called once per frame
    void Update()
    {
        //! lazy way of doing it first no physics just go in a direction
        transform.position += m_direction * m_speed * Time.deltaTime;
        //! range check
        var deltaVec = transform.position - m_startingposition;
        print(deltaVec.magnitude);
        if (deltaVec.magnitude >= m_range) 
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("FX"))
            return;

        if(collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<BaseEnemyBehaviour>().DealDamage(m_damageValue);
        }

        //!Temp
        var go = Instantiate(m_impactPrefab, transform.position, Quaternion.identity);

        Destroy(go, .9f);

        EOnImpact?.Invoke();

        if(++m_currPenCount >= MaxPenCount)
            Destroy(gameObject);
    }

    public void Setup(float dmg, float speed, float lifetime, float range)
    {
        m_damageValue = dmg;
        m_speed = speed;
        m_lifeTime = lifetime;
        m_range = range;
        m_startingposition = transform.position;
    }

    public void SetTravelDirection(Vector2 direction)
    {
        m_direction = direction;
    }

    public void ApplyImpactModifiers(UnityEvent e)
    {
        EOnImpact = e;
    }
}
