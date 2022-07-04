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
    }

    void FixedUpdate()
    {
        Walk();

        if(jumpInput > 0)
        {
            Jump();    
        }
        jumpGravtity();
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
    void Jump()//점프한다
    {
        if(pCollider.onGround)
        {
            rb.AddForce(Vector2.up * jumpForce,ForceMode2D.Impulse);
            isJumping = true;
        }
    }
    void jumpGravtity()//낙하가속
    {
        float gravityScale=1;
        float fallgravityMultiplier=2f;
        if(rb.velocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallgravityMultiplier;
        }
        else
        {
            rb.gravityScale = gravityScale;
        }
    }
}
