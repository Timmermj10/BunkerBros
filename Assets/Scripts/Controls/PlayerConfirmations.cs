using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerConfirmations : MonoBehaviour
{

    private StoryDisplayer storyDisplayer;

    void Start()
    {
        storyDisplayer = GameObject.Find("StoryText").GetComponent<StoryDisplayer>();

        PlayerInput playerInputs = GetComponent<PlayerInput>();

        if (playerInputs != null && gameObject.name is "ActivePlayerControls")
        {
            if (Gamepad.current != null)
            {
                playerInputs.SwitchCurrentControlScheme("ControllerPlayer", Gamepad.current);
            }
            else
            {
                Debug.LogError("No gamepad connected for activePlayer.");
            }
        }

        if (playerInputs != null && gameObject.name is "ManagerPlayerControls")
        {
            if (playerInputs != null)
            {
                playerInputs.SwitchCurrentControlScheme("KBMPlayer", Keyboard.current, Mouse.current);
            }
        }
    }


    private void OnEnterConfirm()
    {
        Debug.Log("OnEnterConfirm called");
        storyDisplayer.player1Confirm();
    }

    private void OnGamepadConfirm()
    {
        Debug.Log("OnGamepadConfirm called");
        storyDisplayer.player2Confirm();
    }
}
