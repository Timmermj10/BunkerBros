using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PopUpSystem : MonoBehaviour
{
    public GameObject popUpBox;
    public GameObject popUpBoxPrefab;


    //Variables to freeze players
    [SerializeField]
    private InputActionAsset actionAsset;
    private InputActionMap managerActionMap;
    private ActivePlayerInputs activePlayerInputs;


    private void Start()
    {
        EventBus.Subscribe<PopUpStartEvent>(_popUpStart);
        EventBus.Subscribe<PopUpEndEvent>(_popUpEnd);
        EventBus.Subscribe<PlayerRespawnEvent>(_setActivePlayerInputs);
        managerActionMap = actionAsset.FindActionMap("ManagerPlayer");
    }

    private void _popUpStart(PopUpStartEvent e)
    {
        //Debug.Log("PopUpCalled");
        popUpBox = Instantiate(popUpBoxPrefab);
        freezePlayer(e.player);
        PopUpPrefabController prefab = popUpBox.GetComponent<PopUpPrefabController>();
        prefab.printText(e.player, e.text);
    }

    private void freezePlayer(string playerToFreeze)
    {
        if (playerToFreeze == "Manager")
        {
            //Debug.Log("Disabling manager Player");
            managerActionMap.Disable();
        }
        else if (playerToFreeze == "Player")
        {
            //Debug.Log("Disabling active Player");
            activePlayerInputs.disableControls();
        }
        else
        {
            //Debug.Log("Disabling both players");
            managerActionMap.Disable();
            activePlayerInputs.disableControls();
        }
    }

    private void _popUpEnd(PopUpEndEvent e)
    {
        //Debug.Log("Turning Controls back on");
        managerActionMap.Enable();
        if (activePlayerInputs != null)
        {
            activePlayerInputs.enableControls();
        }
    }

    private void _setActivePlayerInputs(PlayerRespawnEvent e)
    {
        activePlayerInputs = e.activePlayer.GetComponent<ActivePlayerInputs>();
    }

}
