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
    public float LastOnGroundTime { get; private set; }
    #endregion

    #region INPUT PARAMETERS
	public float LastPressedJumpTime { get; private set; }
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

    [Space]
    [Header("const")]
    const int minimumJumpForce = 1;
    const float defaultJumpForce = 2;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        pCollider = GetComponent<Player_collider>();
        pInput = GetComponent<Player_input>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        #region SETUP INPUTS
        #endregion
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = pInput.movementInput.x;
        jumpInput = pInput.movementInput.y;

        #region TIMERS
        LastOnGroundTime-=Time.deltaTime;
        #endregion

        #region PHYSICS CHECKS
        //Player_colider.cs
        if(pCollider.onGround)
        {
            LastOnGroundTime = coyoteTime; 
        }
        #endregion

        #region GRAVITY
        JumpGravtity();
        #endregion

        #region JUMP CHECKS
        if(isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        } 
        #endregion
    }
    void FixedUpdate()
    {
        Walk();
        
        if(jumpInput > 0 && CanJump())
        {
            isJumping = true;
            Jump(Vector2.up);          
        }
        JumpCut();
    }

    #region BEHAVIOR
    void Walk()//?????????
    {
        float velPower = 1f;
        float pSpeed = moveInput * moveSpeed;
        float speedDif = pSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(pSpeed)>0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate , velPower) * Mathf.Sign(speedDif);
        rb.AddForce(movement * Vector2.right);
    }
    void Jump(Vector2 dir)//????????????
    {
        float force = jumpForce;
	    if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }
        rb.AddForce(dir * force,ForceMode2D.Impulse);
    }
    #endregion

    #region ACTIONABLE
    public bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping;
    }
    private bool CanJumpCut()
    {
		return isJumping && rb.velocity.y > 0;
    }
    #endregion

    void JumpGravtity()//????????????
    {
        float gravityScale=1;
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * 2;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }
    void JumpCut()//?????? ????????? ??? ????????????
    { 
        if(CanJumpCut())
        {
            isJumping=false;
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
}
