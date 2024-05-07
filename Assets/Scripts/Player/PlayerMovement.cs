using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float MoveSpeed = 5f;
    private Rigidbody2D m_rb;
    private Vector2 m_movement;
    private float m_RotationAngle;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //! seem to be the most common suggested wayt o move a top down player movement
        m_rb.MovePositionAndRotation(m_rb.position + m_movement * MoveSpeed * Time.fixedDeltaTime, m_RotationAngle);
        m_rb.rotation = m_RotationAngle
    }

    private void OnMove(InputValue value)
    {
        m_movement = value.Get<Vector2>();
    }

    //private void OnLookAnalog(InputValue value)
    //{
    //    Vector2 lookAtRotation = value.Get<Vector2>();
    //    m_RotationAngle = Mathf.Atan2(lookAtRotation.y, lookAtRotation.x) * Mathf.Rad2Deg - 90f;
    //}

    private void OnLookMouse(InputValue value)
    {
        Vector2 lookAtRotation = value.Get<Vector2>() - m_rb.position;
        m_RotationAngle = Mathf.Atan2(lookAtRotation.y, lookAtRotation.x) * Mathf.Rad2Deg - 90f;
    }
}
