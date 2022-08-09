using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Player_input : MonoBehaviour
{
    public static Player_input instance;
    private InputHandler controls;
    
    #region ACTION
    [SerializeField]
    private InputActionReference movement;
    [SerializeField]
    private InputActionReference sprint;
    #endregion

    #region InputValue
    public Vector2 movementInput { get; private set; }
    public float sprintInput { get; private set; }
    #endregion
    
    // Start is called before the first frame update
    void Awake() 
    {
        #region Assign Inputs
        #endregion
    } 
    void Start()
    {
        
    } 
    // Update is called once per frame
    void Update()
    {
        movementInput = movement.action.ReadValue<Vector2>();
        sprintInput = sprint.action.ReadValue<float>();
        Debug.Log(sprintInput);
    }

    #region Events
    //Events
	#endregion

    #region OnEnable/OnDisable
    private void OnEnable() 
    {

    }
    private void OnDisable() 
    {
        
    }
    #endregion
}
