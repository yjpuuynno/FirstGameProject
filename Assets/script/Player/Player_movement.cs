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
    [Header("Stats")]
    public float moveSpeed = 10;
    public float acceleration = 5;
    public float decceleration = 5;
    public float jumpForce = 1;
    [Space]
    [Header("BoolStats")]
    public bool isJumping = false;
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
        if(isJumping && rb.velocity.y < 0)
        {
            isJumping = false;
        }
        onJumpUp();
        jumpGravtity();
    }

    void FixedUpdate()
    {
        Walk();
        if(jumpInput > 0)
        {
            Jump(Vector2.up);
            isJumping = true; 
        }   
    }

    void Walk()//걷는다
    {
        float velPower = 1f;
        float pSpeed = moveInput * moveSpeed;
        float speedDif = pSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(pSpeed)>0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate , velPower) *Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }
    void Jump(Vector2 dir)//점프한다
    {
        if(pCollider.onGround)
        {
            rb.AddForce(dir * jumpForce,ForceMode2D.Impulse);
        }
    }
    void jumpGravtity()//낙하가속
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
    void onJumpUp()//길게 누르면 더 높게점프
    { 
    float fallMultiplier = 2.5f;
    float lowJumpMultiplier = 2f;
        if(rb.velocity.y < 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }else if(rb.velocity.y > 0 && jumpInput == 0)
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }
}
