using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class ManagerPlayerInputs : MonoBehaviour
{
    private Vector2 movementInputValue;

    private void OnMove(InputValue value)
    {
        movementInputValue = value.Get<Vector2>();
        Debug.Log("Manager Player: MovementInputValue = " + movementInputValue);
    }
}

/*
 * 
 *     private void Awake()
    {
        input = new UserInputs();
    }


    private void OnEnable()
    {
        input.Enable();
        input.ManagerPlayer.ManagerMovement.performed += OnMovementPerformed;
        input.ManagerPlayer.ManagerMovement.performed += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.ManagerPlayer.ManagerMovement.performed -= OnMovementPerformed;
        input.ManagerPlayer.ManagerMovement.performed -= OnMovementCancelled;
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        movementInputValue = value.ReadValue<Vector2>();
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        movementInputValue = Vector2.zero;
    }

    private void FixedUpdate()
    {
        Debug.Log("Manager Player: movementInputValue = " + movementInputValue);
    }*/


