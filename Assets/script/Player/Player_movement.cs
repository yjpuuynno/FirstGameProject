using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    Player_collider pCollider;
    Player_input pInput;

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
    [Header("States")]
    public bool isJumping = false;
    public float LastOnGroundTime ;
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
    }

    // Update is called once per frame
    void Update()
    {
        moveInput = pInput.xRaw;
        jumpInput = pInput.yRaw;

        LastOnGroundTime-=Time.deltaTime;

        if(isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }

        if(pCollider.onGround)
        {
            LastOnGroundTime = coyoteTime; 
        }

        JumpGravtity();
    }
    void FixedUpdate()
    {
        Walk();
        
        if(jumpInput > 0 && CanJump())
        {
            //OnJump();
            isJumping = true;
            Jump(Vector2.up);          
        }
        OnJumpUp();
    }
    void Walk()//걷는다
    {
        float velPower = 1f;
        float pSpeed = moveInput * moveSpeed;
        float speedDif = pSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(pSpeed)>0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate , velPower) * Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }
    //jump한다
    void Jump(Vector2 dir)//점프한다
    {       
        float force = jumpForce;
	    if (rb.velocity.y < 0)
        {
            force -= rb.velocity.y;
        }
        rb.AddForce(dir * force,ForceMode2D.Impulse);
    }
    void JumpGravtity()//낙하가속
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
    void OnJumpUp()//길게 누르면 더 높게점프
    { 
        if(isJumping && rb.velocity.y > 0)
        {
            isJumping=false;
            rb.AddForce(Vector2.down * rb.velocity.y * (1 - jumpCutMultiplier), ForceMode2D.Impulse);
        }
    }
    public bool CanJump()
    {
        return LastOnGroundTime > 0 && !isJumping;
    }
}
