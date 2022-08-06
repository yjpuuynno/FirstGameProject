using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    Player_collider pCollider;
    Player_input pInput;
    private Animator animator;

    #region STATE PARAMETERS
    public bool isJumping { get; private set; }
    public bool isHanging { get; private set; }
    public bool isLedgeClimb { get; private set; }
    public float LastOnGroundTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
	public float LastPressedJumpTime;
    public float LastPressedMoveTime;
    [Range(0, 1)] public float moveInputWaitTime = 0.3f;
	#endregion

    [Space]
    [Header("Input")]
    private float moveInput;
    bool moveInputDown;
    bool moveInputUp;
    private bool moveInputPass;
    public int moveInputCount = 1;
    private float jumpInput;
    

    [Space]
    [Header("Status")]
    public float moveSpeed;
    const float walkingSpeed = 2.5f;
    const float runingSpeed = 6;
    public float acceleration = 4;
    public float decceleration = 5;
    public float jumpForce;
    [Range(0, 1)] public float jumpCutMultiplier;
    [Range(0, 0.5f)] public float coyoteTime;
    [Range(1f, 5f)] public float runingStartTime;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<Player_collider>();
        pInput = GetComponent<Player_input>();
        animator = GetComponent<Animator>();

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = pInput.movementInput.x;
        moveInputDown = (moveInput != 0);
        moveInputUp = (moveInput == 0);

        jumpInput = pInput.movementInput.y;
        
        #region TIMERS
        LastOnGroundTime-=Time.deltaTime;
        LastPressedJumpTime-=Time.deltaTime;
        #endregion

        #region INPUTCOUNT

        if(moveInputUp)
        {
            moveInputPass = true;
        }
        if(moveInputPass)
        {
            if(moveInputDown && (Time.time - LastPressedMoveTime) < moveInputWaitTime)
            {
                moveInputCount += 1;
                moveInputPass = false;
            }else if(moveInputDown)
            {
                moveInputPass = false;
                LastPressedMoveTime = Time.time;
                moveInputCount = 1;
            }
        }
        #endregion

        #region PHYSICS CHECKS
        if(pCollider.onGround)
        {
            LastOnGroundTime = coyoteTime; 
        }
        #endregion

        #region GRAVITY CHECKS
        if(CanMove())
        {
            SetGravtityScale(1);
        }
        #endregion

        #region STATUS CHECKS
        
        #endregion

        #region BEHAVIOR CHECKS
        if(isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
        #endregion
    }
    void FixedUpdate()
    {
        if(CanMove())
        {
            Walk();
        }
        
        if(CanJump())
        {
            isJumping = true;
            Jump(Vector2.up,jumpForce);          
        }
        if(CanLedgeClimb())
        {
            StartCoroutine(LedgeClimb());
        }  
        
        JumpCut();
        
        #region DEBUG LOG
        #endregion
    }

    #region BEHAVIOR
    void Walk()//걷는다
    {
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
    IEnumerator LedgeClimb()
    {
        isLedgeClimb=true;
        SetGravtityScale(0);
        animator.SetBool("DoLedgeclimb",isLedgeClimb);
        rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(1);
        //if(animator.GetCurrentAnimatorStateInfo(0).IsName("Base Layer.LedgeClimb") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
        //{
        isLedgeClimb=false;
        SetGravtityScale(1);
        animator.SetBool("DoLedgeclimb",isLedgeClimb);
        transform.position = new Vector2(transform.position.x + (0.43f * pCollider.wallSide),transform.position.y + 1.932f);
        //}
    }
    #endregion

    #region ACTIONABLE
    private bool CanMove()
    {
        return !isLedgeClimb;
    }
    public bool CanJump()
    {
        bool input = (jumpInput > 0);
        return CanMove() && input && LastOnGroundTime > 0 && !isJumping;
    }
    private bool CanJumpCut()
    {
		return isJumping && rb.velocity.y > 0;
    }
    private bool CanRuning()
    {
        return CanMove() && moveInputCount > 1;
    }
    private bool CanLedgeClimb()
    {
        bool input = (moveInputUp && jumpInput > 0);
        return CanMove() && input && pCollider.onLedge && (pCollider.wallSide > 0 == moveInput > 0) && !isLedgeClimb;
    }
    #endregion
    void SetGravtityScale(int gravityScale)//중력 스케일 증가
    {
        rb.gravityScale = gravityScale;
    }
    void JumpCut()//길게 누르면 더 높게점프
    { 
        if(CanJumpCut())
        {
            isJumping=false;
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
    #region API
    /*
      private float GetAnimLength(string animName)
    {
        float time = 0;
        RuntimeAnimatorController ac = animator.runtimeAnimatorController;
 
        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                time = ac.animationClips[i].length;
            }
        }
 
        return time;
    }
    */
    #endregion
}
