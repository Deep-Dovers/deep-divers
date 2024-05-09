using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public PlayerController Owner { get; private set; }

    [SerializeField]
    private float m_moveSpeed = 5f;

    [SerializeField]
    private float m_linearDrag = 1f;

    private Rigidbody2D m_rb;
    private Vector2 m_movement;
    private bool isFaceingRight;

    public float jumpTime = 0.35f;
    public float jumpTimeCounter;

    [Header ("Movement Varibles")]
    [SerializeField]
    float MaxSpeedHorizontal;
    [SerializeField]
    float MaxSpeedVertical;
    [SerializeField]
    float SlowDownSpeed;
    [SerializeField]
    float RunningStartSpeed;
    [SerializeField]
    float RunningMaxSpeed;

    [Space]
    [Header("Jump Varibles")]
    [SerializeField]
    float m_jumpForce = 20f;
    [SerializeField]
    float JumpForceInitial;
    [SerializeField]
    float JumpForceHoldIncrement;
    
    [Space]
    [Header("Not yet done")]
  
    [SerializeField]
    float DashSpeed;
  
    float AirControlsStrength;
    [SerializeField]
    float Gravity;
    [SerializeField]
    float SlowFallSpeed;
    [SerializeField]
    int MaxJumpCount;
    [SerializeField]
    float JumpCooldown;
    [SerializeField]
    int MaxDashCount;
    [SerializeField]
    float DashCooldown;
    [SerializeField]
    string Role;
    [SerializeField]
    string Race;


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
        m_rb.velocity = new Vector2(m_moveSpeed * m_movement.x, m_rb.velocity.y);
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

    public void OnAttackInput(bool isAttackPressed = true)
    {
        print("I am attacking");
    }

    public void OnMoveInput(Vector2 value)
    {
        //THIS IS VERY PLACEHOLDER
        transform.position += new Vector3(value.x, value.y, 0f)/* * Time.deltaTime*/;
    }

    public void SetOwner(PlayerController owner)
    {
        print("My new owner is player index " + owner.PlayerIndex);
        Owner = owner;
    }
}
