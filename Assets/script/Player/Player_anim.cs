using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_anim : MonoBehaviour
{
    private Animator animator;
    private Player_movement player_Movement;
    private Player_input player_Input;
    private Rigidbody2D rb; 
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        player_Movement = GetComponent<Player_movement>();
        player_Input = GetComponent<Player_input>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player_Input.movementInput.x != 0)
        {
            Flip();
            animator.SetFloat("Speed",Mathf.Abs(player_Input.movementInput.x));
        }
    }
    void Flip()
    {
        Vector2 currentScale = transform.localScale;
        currentScale.x = player_Input.movementInput.x < 0 ? -1 : 1;
        transform.localScale = currentScale;
    }
}