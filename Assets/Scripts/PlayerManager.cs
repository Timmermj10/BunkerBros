using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public GameObject activePlayerPrefab;

    public GameObject activePlayer;
    private GameObject managerPlayer;

    void Start()
    {

        EventBus.Subscribe<ObjectDestroyedEvent>(_PlayerDead);

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

    private void _PlayerDead(ObjectDestroyedEvent e)
    {
        if (e.tag == "Player")
        {

            StartCoroutine(RespawnPlayer());
        }
    }

    IEnumerator RespawnPlayer()
    {

        yield return new WaitForSeconds(3);

        Debug.Log("Respawning Player");

        GameObject activePlayer = Instantiate(activePlayerPrefab, new Vector3(0, 1, -2), Quaternion.identity);

        PlayerInput activePlayerInput = activePlayer.GetComponent<PlayerInput>();
        if (Gamepad.current != null)
        {
            activePlayerInput.SwitchCurrentControlScheme("ControllerPlayer", Gamepad.current);
        }
        else
        {
            Debug.LogError("No gamepad connected for activePlayer.");
        }
        EventBus.Publish(new PlayerRespawnEvent(activePlayer.transform.position, activePlayer));

        yield return null;
    }



}
