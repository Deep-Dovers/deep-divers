using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float m_moveSpeed = 5f;

    [SerializeField]
    private float m_linearDrag = 1f;

    private Rigidbody2D m_rb;
    private Vector2 m_movement;
    private float m_RotationAngle;
    private bool isFaceingRight;

    public float m_jumpForce = 20f;
    public float jumpTime = 0.35f;
    public float jumpTimeCounter;

    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //! seem to be the most common suggested wayt o move a top down player movement
        ApplyMovement();
    }
  
    private void Jump()
    {
        m_rb.velocity = new Vector2(m_rb.velocity.x, m_jumpForce);
    }
    private void ApplyMovement()
    {
        m_rb.velocity = new Vector2 (m_moveSpeed * m_movement.x, m_rb.velocity.y);
    }

    //! listen for input
    private void OnMove(InputValue value)
    {
        Debug.Log("moving" + value);
        m_movement = value.Get<Vector2>();
    } 
    private void OnJumpInput(bool isJumpPressed = true)
    {
        Debug.Log("Jump");

        Jump();
    }
}
