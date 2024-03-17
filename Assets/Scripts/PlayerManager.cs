using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    // Start is called before the first frame update

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

        //Instantiate the player with a keyboard and mouse
        // GameObject managerPlayer = Instantiate(managerPlayerPrefab, new Vector3(-1f, 1f, 0f), Quaternion.identity);
        managerPlayer = GameObject.Find("ManagerCamera");
        PlayerInput managerPlayerInput = managerPlayer.GetComponent<PlayerInput>();
        //Assuming you have created a control scheme for Keyboard&Mouse in your Input Actions called "KeyboardMouseScheme"
        if (managerPlayerInput != null)
        {
           managerPlayerInput.SwitchCurrentControlScheme("KBMPlayer", Keyboard.current, Mouse.current);
        }
    }

}
