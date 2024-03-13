using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserInputTest : MonoBehaviour
{

    public GameObject activePlayerPrefab;
    public GameObject managerPlayerPrefab;

    private GameObject activePlayer;
    private GameObject managerPlayer;

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

}
