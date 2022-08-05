using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_anim : MonoBehaviour
{
    private Animator animator;
    private Player_movement player_Movement;
    private Player_input player_Input;
    private Player_collider player_Collider;
    private Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player_Movement = GetComponent<Player_movement>();
        player_Input = GetComponent<Player_input>();
        player_Collider = GetComponent<Player_collider>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("DoLedgeclimb",player_Movement.isLedgeClimb);
        animator.SetFloat("Speed",Mathf.Abs(rb.velocity.x));
        animator.SetFloat("OnAir",rb.velocity.y);
        animator.SetBool("onGround", player_Collider.onGround);
        
        if(player_Input.movementInput.x != 0 && !player_Movement.isLedgeClimb)
        {
            Flip();
        }   
    }
    void Flip()
    {
        Vector2 currentScale = transform.localScale;
        currentScale.x = player_Input.movementInput.x < 0 ? -1 : 1;
        transform.localScale = currentScale;
    }
}
