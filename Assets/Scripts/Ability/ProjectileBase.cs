using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Update is called once per frame
    void Update()
    {
        //! lazy way of doing it first no physics just go in a direction
        transform.position += m_direction * m_speed * Time.deltaTime;

        //! range check
        var deltaVec = transform.position - m_startingposition;
        if (deltaVec.magnitude >= m_range) 
        {
            Destroy(gameObject);
        }
    }
    public void InitProjectile(Vector2 direction)
    {
        m_startingposition = transform.position;
        Destroy(gameObject, m_lifeTime);
        m_direction = direction;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Destroy(gameObject);
    }

    public void Setup(float dmg, float speed, float lifetime, float range)
    {
        m_damageValue = dmg;

        m_speed = speed;
        m_lifeTime = lifetime;
        m_range = range;
    }

    public void SetTravelDirection(Vector2 direction)
    {
        m_direction = direction;
    }
}
