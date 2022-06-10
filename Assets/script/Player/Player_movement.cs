using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_movement : MonoBehaviour
{
    public Rigidbody2D rigidbody2D;
    private Player_collider player_Collider;
    private Player_input player_Input;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        player_Collider = GetComponent<Player_collider>();
        player_Input = GetComponent<Player_input>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        
    }

    void Walk()//걷는다
    {/*
        if(현제속력<최대속력){
        현제속력+=가속도;
        }
        */
    }
    void Jump()//점프한다
    {/*
        길게 누르면 조금 더 높게 점프 한다.
        */
    }
}
