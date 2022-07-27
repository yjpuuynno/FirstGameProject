using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    Player_collider pCollider;
    Player_input pInput;

    #region STATE PARAMETERS
    public bool isJumping { get; private set; }
    public bool isHanging { get; private set; }
    public float LastOnGroundTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
	public float LastPressedJumpTime; //{ get; private set; }
    public float LastPressedWalkTime; //{ get; private set; }
	#endregion

    [Space]
    [Header("Input")]
    private float moveInput;
    private float jumpInput;

    [Space]
    [Header("Status")]
    public float moveSpeed = 5;
    public float acceleration = 4;
    public float decceleration = 5;
    public float jumpForce;
    [Range(0, 1)] public float jumpCutMultiplier;
    [Range(0, 0.5f)] public float coyoteTime;
    [Range(1f, 5f)] public float runingStartTime;
    [Range(0, 1)] public float HangingMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<Player_collider>();
        pInput = GetComponent<Player_input>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = pInput.movementInput.x;
        jumpInput = pInput.movementInput.y;

        #region TIMERS
        LastOnGroundTime-=Time.deltaTime;
        LastPressedWalkTime-=Time.deltaTime;
        #endregion

        #region PHYSICS CHECKS
        if(pCollider.onGround)
        {
            LastOnGroundTime = coyoteTime; 
        }
        if(Mathf.Abs(rb.velocity.x) < 0.1f )
        {
            LastPressedWalkTime = runingStartTime;
        }
        #endregion

        #region GRAVITY CHECKS
        JumpGravtity();
        #endregion

        #region STATUS CHECKS
        
        #endregion

        #region BEHAVIOR CHECKS
        if(isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
        if(isHanging && !canHanging())
        {
            isHanging  = false;
        }
        #endregion
    }
    void FixedUpdate()
    {
        Walk();
        if(jumpInput > 0 && CanJump())
        {
            isJumping = true;
            Jump(Vector2.up,jumpForce);          
        }

        JumpCut();
        
        #region DEBUG LOG
        #endregion
    }

    #region BEHAVIOR
    void Walk()//걷는다
    {
        float walkingSpeed = 2.5f;
        float runingSpeed = walkingSpeed * walkingSpeed;
        moveSpeed = CanRuning() ? runingSpeed : walkingSpeed;
        float velPower = 1f;
        float pSpeed = moveInput * moveSpeed;
        float speedDif = pSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(pSpeed)>0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate , velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
    }
    void Jump(Vector2 dir,float force)
    {
	    if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }
        rb.AddForce(dir * force,ForceMode2D.Impulse);
    }
    void LedgeHanging()
    {
        isHanging = true;
        rb.velocity = Vector2.zero;
    }
    void LedgeUp() 
    {
    }
    #endregion

    #region ACTIONABLE
    private bool canHanging()
    {
        return pCollider.onLedge && !pCollider.onGround && ((pInput.movementInput.x>0) == (pCollider.wallSide>0));
    }
    public bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping;
    }
    private bool CanJumpCut()
    {
		return isJumping && rb.velocity.y > 0;
    }
    private bool CanRuning()
    {
        return LastPressedWalkTime < 0;
    }
    #endregion

    void JumpGravtity()//낙하할때 중력 스케일 증가
    {
        float gravityScale=1;
        rb.gravityScale = rb.velocity.y < 0 ? gravityScale * 2 : gravityScale;
    }
    void JumpCut()//길게 누르면 더 높게점프
    { 
        if(CanJumpCut())
        {
            isJumping=false;
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
}
