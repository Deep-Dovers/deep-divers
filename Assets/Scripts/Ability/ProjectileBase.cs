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

    private Vector3 m_direction;
    private Vector3 m_startingposition;
    // Start is called before the first frame update
    void Start()
    {
        
    }

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
}
