using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerCharacter : MonoBehaviour
{
    [field: SerializeField, ReadOnly]
    public PlayerController Owner { get; private set; }

    [SerializeField]
    private float m_linearDrag = 1f;

    //! private fields
    private Rigidbody2D m_rb;
    private Vector2 m_movement;
    private bool isFaceingRight;
    private bool m_isGrounded;
    private int m_jumpCount = 0;
    private float jumpTime = 0.35f;
    private float jumpTimeCounter = 0;
    private float lastGroundedTime = 0f;
    private float lastjumpTime = 0f;
    private bool m_isJumping = false;
    private bool jumpInputReleased = false;
 

    [Header ("Movement Variable")]
    [SerializeField, Tooltip("The max Horizontal move speed for the player")]
    private float m_maxMoveSpeed = 10f;  // Player Max Speed Horizontal
    [SerializeField, Tooltip("If accleration is bigger then max speed he player will immedietly hit max velocity")]
    private float m_acceleration = 5;    // Player Running Start Speed
    [SerializeField, Tooltip("the decceleration rate of the player")]
    private float m_decceleration = 5f;  // Player Slow Down Speed
    [SerializeField, Range(0,1), Tooltip("this value is used to make the player feel more snappy when turning, the lower the number the less snappy")]
    private float m_velPow = 0.9f;
    [SerializeField, Range(0, 1)]
    private float m_frictionValue = 0.2f;
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
    int m_maxJumpCount = 1;
    [SerializeField]
    float m_jumpCooldown;
    [SerializeField]
    float m_maxFallSpeed = 100f;
    [Header("Is ground check values")]
    [SerializeField]
    private Vector2 boxSize;
    [SerializeField]
    private float castDistance;
    [SerializeField]
    private LayerMask GroundLayer;

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

        //! Implemeted some tips from this video here https://www.youtube.com/watch?v=2S3g8CgBG1g to make the jump feel better
        #region Jump
        if (m_rb.velocityY < 0)
        {
            m_rb.gravityScale = 3f;
        }
        //! Cap the falling speed
        if(Mathf.Abs(m_rb.velocityY) >= m_maxFallSpeed)
        {
            m_rb.velocityY = Mathf.Sign(m_rb.velocityY) * m_maxFallSpeed;
        }
        #endregion
    }
    private void CheckGrounded()
    {
        if(Physics2D.BoxCast(transform.position, boxSize, 0 , -transform.up, castDistance, GroundLayer))
        {
            //! reset jump values
            m_isGrounded = true;
            m_isJumping = false;
            m_jumpCount = 0;
            m_rb.gravityScale = 2f;
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
        Debug.Log("Jumping");
        m_rb.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        lastGroundedTime = 0;
        lastjumpTime = 0f;
        m_isJumping = true;
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

    private void Dash()
    {

    }

    private void ApplyFriction()
    {
        //! Make sure we are grounded and there is currently forward or back is not being pressed
        if(m_isGrounded && Mathf.Abs(m_movement.x) < 0.01)
        {
            //! See which is smaller currently my velocity or the friction value
            float frictionVal = Mathf.Min(Mathf.Abs(m_rb.velocityX), Mathf.Abs(m_frictionValue));
            //! Add back the direction
            frictionVal *= Mathf.Sign(m_rb.velocityX);
            //! Apply the friction against my current movement direction
            m_rb.AddForceX(-frictionVal, ForceMode2D.Impulse);
        }
    }

    //! listen for input
    public void OnJumpInput(bool isJumpPressed = true)
    { 
        if(m_isGrounded || m_jumpCount < m_maxJumpCount)
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
    public void OnDashInput(bool isDashPressed = true)
    {
        Debug.Log("Dash input received");
        Dash();
    }
    public void SetOwner(PlayerController owner)
    {
        print("My new owner is player index " + owner.PlayerIndex);
        Owner = owner;
    }
}
