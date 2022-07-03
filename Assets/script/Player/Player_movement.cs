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
    [Space]
    [Header("Stats")]
    public float moveSpeed = 10;
    public float acceleration = 5;
    public float decceleration = 5;
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
    }

    void FixedUpdate()
    {
        Walk();
    }

    void Walk()//걷는다
    {/*
        if(현제속력<최대속력){
        현제속력+=가속도;
        }
        */
        float velPower = 1f;
        float pSpeed = moveInput * moveSpeed;
        float speedDif = pSpeed - rb.velocity.x;
        float accelRate = (Mathf.Abs(pSpeed)>0.01f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDif)*accelRate , velPower) *Mathf.Sign(speedDif);

        rb.AddForce(movement * Vector2.right);
    }
    void Jump()//점프한다
    {/*
        길게 누르면 조금 더 높게 점프 한다.
        */
    }
}
