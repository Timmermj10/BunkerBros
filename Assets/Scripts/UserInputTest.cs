using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserInputTest : MonoBehaviour
{

    public GameObject activePlayerPrefab;
    public GameObject managerPlayerPrefab;

    public GameObject activePlayer;
    private GameObject managerPlayer;

    void Start()
    {
        // Instantiate the player with a gamepad
        //GameObject activePlayer = Instantiate(activePlayerPrefab, new Vector3(1f, 1f, 0f), Quaternion.identity);
        PlayerInput activePlayerInput = activePlayer.GetComponent<PlayerInput>();
        if (Gamepad.current != null)
        {
            activePlayerInput.SwitchCurrentControlScheme("ControllerPlayer", Gamepad.current);
        }
        else
        {
            Debug.LogError("No gamepad connected for activePlayer.");
        }

        // Instantiate the player with a keyboard and mouse
        //GameObject managerPlayer = Instantiate(managerPlayerPrefab, new Vector3(-1f, 1f, 0f), Quaternion.identity);
        PlayerInput managerPlayerInput = managerPlayer.GetComponent<PlayerInput>();
        // Assuming you have created a control scheme for Keyboard&Mouse in your Input Actions called "KeyboardMouseScheme"
        if (managerPlayerInput == null)
        {
            managerPlayerInput.SwitchCurrentControlScheme("KBMPlayer", Keyboard.current, Mouse.current);
        }
    }

}

/*
 
    void Start()
    {
        // Ensure two gamepads are connected
        if (Gamepad.all.Count < 2)
        {
            Debug.LogError("Not enough gamepads connected.");
            return;
        }

        // Instantiate the first player
        activePlayer = Instantiate(activePlayerPrefab, new Vector3(1f, 1f, 0f), Quaternion.identity);
        PlayerInput activePlayerInput = activePlayer.GetComponent<PlayerInput>();
        activePlayerInput.SwitchCurrentControlScheme("PlayerControls", Gamepad.all[0]);

        // Instantiate the second player
        managerPlayer = Instantiate(managerPlayerPrefab, new Vector3(-1f, 1f, 0f), Quaternion.identity);
        PlayerInput managerPlayerInput = managerPlayer.GetComponent<PlayerInput>();
        managerPlayerInput.SwitchCurrentControlScheme("PlayerControls", Gamepad.all[1]);

    }

*/
