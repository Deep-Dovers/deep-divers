using NaughtyAttributes;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.InputSystem;

public class PlayerCharacter : NetworkBehaviour
{
    [field: SerializeField, ReadOnly]
    public PlayerController Owner { get; private set; }

    //! private fields / Debugging Values
    private Rigidbody2D m_rb;
    private bool m_isFaceingRight = true;

    #region Movement variable
    [Header("Movement Variable")]
    [SerializeField, Tooltip("Fastest possible speed the player can move at")]
    private float m_maxHorizontalVelocity;
    [SerializeField, Tooltip("The max movement speed")]
    private float m_maxMoveSpeed = 10f;  // Player Max Speed Horizontal
    [SerializeField, Tooltip("If accleration is bigger then max speed he player will immedietly hit max velocity")]
    private float m_acceleration = 5;    // Player Running Start Speed
    [SerializeField, Tooltip("The decceleration rate of the player")]
    private float m_decceleration = 5f;  // Player Slow Down Speed
    [SerializeField, Range(0, 1), Tooltip("This value is used to make the player feel more snappy when turning, the lower the number the less snappy")]
    private float m_velPow = 0.9f;
    [SerializeField, Range(0, 1), Tooltip("Increase the value if u want the player to not slide so much when stopping")]
    private float m_frictionValue = 0.2f;

    private Vector2 m_movement;
    private Vector2 m_currentVelocity;   //! for debugging 
    #endregion

    #region Jump variable
    [Space]
    [Header("Jump Variable")]
    [SerializeField, Tooltip("The force that is applied to a rigidbody when jumping")]
    float m_jumpForce = 5f;
    [SerializeField, Tooltip("The number of time the player can jump before it has to be grounded")]
    int m_maxJumpCount = 1;
    [SerializeField, Tooltip("When the player jump is on CD the player cannot jump again and if the jump button is released duriing CD it will trigger a jump cut")]
    float m_jumpCooldown = 0.5f;
    [SerializeField, Tooltip("Max falling speed of the player")]
    float m_maxFallSpeed = 50f;
    [SerializeField, Tooltip("The time allowance for player to jump when walking off a platform")]
    float m_coyoteTime = 0.3f;
    [SerializeField, Tooltip("Player default gravity scale")]
    private float m_gravityDefault = 2f;
    [SerializeField, Tooltip("The falling gravity once player hit apex of the jump")]
    private float m_fallingGravity = 3f;
    [SerializeField, Tooltip("The gravity scale that will be applied when player released the jump button duuring mid jump")]
    private float m_jumpCutGravity = 6f;
    [Header("Ground check values")]
    [SerializeField, Tooltip("Size of the box for the box cast to check grounded")]
    private Vector2 boxSize;
    [SerializeField, Tooltip("The cast distance of the box cast")]
    private float castDistance;
    [SerializeField, Tooltip("The layer that the player will perform a ground check when the cast hits")]
    private LayerMask GroundLayer;

    private bool m_isGrounded;
    private int m_jumpCount = 0;
    private float m_jumpCDTimer;
    private float m_coyoteTimeCounter;
    private bool m_isJumping = false;
    #endregion

    #region Dash variable
    [Space]
    [Header("Dash Variable")]
    [SerializeField, Tooltip("the velocity that is applied when dash is triggered")]
    private float m_dashSpeed;
    [SerializeField, Tooltip("How may dash the player can perform before the CD")]
    private int m_maxDashCount;
    [SerializeField, Tooltip("How long it takes for the dash to reset")]
    private float m_dashCooldown;

    private int m_dashCount;
    private float m_dashCDTimer;
    private bool m_isDashing = false;
    private bool m_canDash = true;
    private Vector2 m_dashDirection = Vector2.zero;
    #endregion

    [Space]
    [Header("Not yet done")]
    [SerializeField]
    float AirControlsStrength;      //! feels redundent 
    [SerializeField]
    float SlowFallSpeed;            //! Need a convo with designers
    [SerializeField]
    string Role;
    [SerializeField]
    string Race;

    public override void OnNetworkSpawn()
    {
        //! ALL THIS IS TO TEST ITS VERY HACKY 
        Debug.Log("well hello there " + IsLocalPlayer);
        if(IsLocalPlayer)
        {
            var playerController = FindAnyObjectByType<PlayerController>();
            playerController.GetComponent<PlayerController>().m_character = this;
        }
    }
   
    // Start is called before the first frame update
    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
        m_rb.gravityScale = m_gravityDefault;
        m_coyoteTimeCounter = m_fallingGravity;
        m_jumpCDTimer = m_jumpCooldown;
        m_dashCDTimer = m_dashCooldown;
    }
    private void Update()
    {
        //! this is to make sure the ground check does not stright away return true when jumping
        if (m_isJumping)
        {
            m_jumpCDTimer -= Time.deltaTime;
            if (m_jumpCDTimer <= 0)
            {
                m_isJumping = false;
                m_jumpCDTimer = m_jumpCooldown;
            }
        }

        #region Dash
        if (m_isDashing)
        {
            m_dashCDTimer -= Time.deltaTime;
            if (m_dashCDTimer <= 0)
            {
                m_isDashing = false;
                m_dashCount = 0;
                m_canDash = true;
            }
        }
        #endregion
    }
    // Anything that relates to physics is done here
    void FixedUpdate()
    {
        m_currentVelocity = m_rb.velocity;
        //! seem to be the most common suggested wayt o move a top down player movement
        ApplyMovement();
        ApplyFriction();
        CheckGrounded();

        //! Implemeted some tips from this video here https://www.youtube.com/watch?v=2S3g8CgBG1g to make the jump feel better
        #region Jump
        if (m_rb.velocityY < -1)
        {
            m_rb.gravityScale = m_fallingGravity;
        }
        //! Cap the falling speed
        if (Mathf.Abs(m_rb.velocityY) >= m_maxFallSpeed)
        {
            m_rb.velocityY = Mathf.Sign(m_rb.velocityY) * m_maxFallSpeed;
        }
        #endregion
    }
    private void CheckGrounded()
    {
        if (Physics2D.BoxCast(transform.position, boxSize, 0, -transform.up, castDistance, GroundLayer) && !m_isJumping)
        {
            //! reset jump values
            m_isGrounded = true;
            m_jumpCount = 0;                         //! Reset jump count
            m_rb.gravityScale = m_gravityDefault;    //! Set the gravity back to default player gravity scale
            m_coyoteTimeCounter = m_coyoteTime;      //! Reset coyote time
        }
        else
        {
            m_isGrounded = false;
            m_coyoteTimeCounter -= Time.deltaTime;   //! Tick down coyoteTime if not grounded
        }

    }
    private void OnDrawGizmos()
    {
        //! Draws the ground check box cast so can adjust in editor.
        Gizmos.DrawWireCube(transform.position - transform.up * castDistance, boxSize);
    }

    private void Jump()
    {
        Debug.Log("Jumping");
        //m_rb.AddForce(Vector2.up * m_jumpForce, ForceMode2D.Impulse);
        m_rb.velocityY = m_jumpForce;  //! setting the velocity seems to feel better should show desinger both implementation
        m_jumpCount++;
        m_isJumping = true;
    }
    private void JumpCut()
    {
        //! When the jump button is released and the player is still jumping cut the jump height by inceasing the player gravity
        m_rb.gravityScale = m_jumpCutGravity;
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

        //! Cap horizontal speed
        if(m_maxHorizontalVelocity <= Mathf.Abs(m_rb.velocityX))
        {
            m_rb.velocityX = m_maxHorizontalVelocity * Mathf.Sign(m_rb.velocityX);
        }

        //! Make sure to flip the player if there is a change in direction
        if (m_movement.x > 0 && !m_isFaceingRight)
        {
            Flip();
        }
        if (m_movement.x < 0 && m_isFaceingRight)
        {
            Flip();
        }
    }
    private void Flip()
    {
        //! Invert the x scale to flip the player charcter
        Vector3 curScale = gameObject.transform.localScale;
        curScale.x *= -1;
        gameObject.transform.localScale = curScale;
        m_isFaceingRight = !m_isFaceingRight;
    }

    private void Dash()
    {
        if (m_canDash)
        { 
            if (m_isFaceingRight)
            {
                m_rb.velocityX = m_dashSpeed;
            }
            else
            {
                m_rb.velocityX = -m_dashSpeed;
            }
            m_isDashing = true;
            m_dashCDTimer = m_dashCooldown;
            m_dashCount++;
            if(m_dashCount >= m_maxDashCount)
            {
                m_canDash = false;
            }
        }
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
    public void OnJumpInput(bool isPressed)
    { 
        if(isPressed)
        {  
            if (!m_isJumping && (m_isGrounded || m_jumpCount < m_maxJumpCount || m_coyoteTimeCounter > 0))
            {
                Jump();
            }
        }
        else if(!isPressed && m_isJumping)
        {
            Debug.Log("Jump button released");
            JumpCut();
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
