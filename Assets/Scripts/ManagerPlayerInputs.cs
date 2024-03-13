using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class ManagerPlayerInputs : MonoBehaviour
{
    private Vector2 movementInputValue;

    PlayerInput playerInput;

    InputAction moveAction;

    private Camera mainCamera;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

        Debug.Log(playerInput.currentControlScheme);
        Debug.Log(playerInput.currentActionMap);

        mainCamera = Camera.main;
    }


    private void OnMove(InputValue value)
    {
        movementInputValue = value.Get<Vector2>();
        Debug.Log("Manager Player: MovementInputValue = " + movementInputValue);
    }

    private void OnInteract(InputValue value)
    {

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit))
        {
            Vector3 worldPosition = hit.point;
            Debug.Log("Mouse is over the tile at Position: " + new Vector3(Mathf.RoundToInt(worldPosition.x), 0f, Mathf.RoundToInt(worldPosition.z)));
            // Now worldPosition contains the 3D point in world space where the mouse is pointing
        }
        else
        {
            // Optional: Handle the case where the ray does not hit any collider
            Debug.Log("Mouse is over nothing");
        }
    }

}

/*
     private void FixedUpdate()
    {
        movePlayer();
    }

    private void movePlayer()
    {
        if (moveAction.ReadValue<Vector2>() != Vector2.zero)
        {
            Debug.Log(moveAction.ReadValue<Vector2>());
        }
    }
    }
*/


