using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public PlayerController Owner { get; private set; }

    [SerializeField]
    private float m_linearDrag = 1f;

    private Rigidbody2D m_rb;
    private Vector2 m_movement;
    private bool isFaceingRight;
    private bool m_isGrounded;

    public float jumpTime = 0.35f;
    public float jumpTimeCounter = 0;
    public float lastGroundedTime = 0f;
    public float lastjumpTime = 0f;
    public bool isJumping = false;
    public bool jumpInputReleased = false;
    public Vector2 boxSize;
    public float castDistance;
    public LayerMask GroundLayer;

    [Header ("Movement Variable")]
    [SerializeField, Tooltip("The max Horizontal move speed for the player")]
    private float m_maxMoveSpeed = 10f;  // Player Max Speed Horizontal
    [SerializeField, Tooltip("If accleration is bigger then max speed he player will immedietly hit max velocity")]
    private float m_acceleration = 5;    // Player Running Start Speed
    [SerializeField, Tooltip("the decceleration rate of the player")]
    private float m_decceleration = 5f;  // Player Slow Down Speed
    [SerializeField, Range(0,1), Tooltip("this value is used to make the player feel more snappy when turning, the lower the number the less snappy")]
    private float m_velPow = 0.9f;
    [SerializeField]
    float RunningMaxSpeed;

    [Space]
    [Header("Jump Variable")]
    [SerializeField]
    float m_jumpForce = 5f;
    [SerializeField]
    float JumpForceInitial;
    [SerializeField]
    float JumpForceHoldIncrement;
    [SerializeField]
    int MaxJumpCount;
    [SerializeField]
    float JumpCooldown;

    [Space]
    [Header("Not yet done")]
  
    [SerializeField]
    float DashSpeed;
    [SerializeField]
    float AirControlsStrength;
    [SerializeField]
    float Gravity;
    [SerializeField]
    float SlowFallSpeed;
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
        ApplyFriction();
        CheckGrounded();

        //! implemeted some tips from this video here https://www.youtube.com/watch?v=2S3g8CgBG1g to make the jump feel better
        #region Jump
        if (m_rb.velocityY < 0)
            m_rb.gravityScale = 3f; 
        #endregion
    }
    private void CheckGrounded()
    {
        
        if(Physics2D.BoxCast(transform.position, boxSize, 0 , -transform.up, castDistance, GroundLayer))
        {
            m_isGrounded = true;
            m_rb.gravityScale = 1f;
        }
        else
        {
            m_isGrounded = false;
        }
         
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    private void Jump()
    {
        m_rb.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0;
        lastjumpTime = 0f;
        isJumping = true;
        jumpInputReleased = false;
    }
    private void ApplyMovement()
    {
        //! The desired speed we want player to acclerate to
        float maxSpeed = m_movement.x * m_maxMoveSpeed;

        //! The speed dif from my current speed to the desired speed
        float speedDif = maxSpeed - m_rb.velocity.x;

        //! this is to decide where the player is decceleration or accelerationg based of the
        //desired speed based of the input for example if movement.x is 0 means i want to stop so deccleration value is used instead
        float accelRate = (Mathf.Abs(maxSpeed) > 0.01f) ? m_acceleration : m_decceleration;

        // Mulultiplayer the accleration  with a set power so when changing direction its more snappy and also apply back the sign so we know we moving left or right
        float movement  = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, m_velPow) * Mathf.Sign(speedDif);
       
        //! Apply accleration
        m_rb.AddForceX(movement);
    }

    private void ApplyFriction()
    {

    }

    //! listen for input
    public void OnJumpInput(bool isJumpPressed = true)
    {
        Debug.Log("Jump");
        if(m_isGrounded)
        {
            Jump();
        }   
    }

    public void OnAttackInput(bool isAttackPressed = true)
    {
        print("I am attacking");
    }

    public void OnMoveInput(Vector2 value)
    {
        Debug.Log("moving" + value);
        m_movement = value;
    }

    public void SetOwner(PlayerController owner)
    {
        print("My new owner is player index " + owner.PlayerIndex);
        Owner = owner;
    }
}
