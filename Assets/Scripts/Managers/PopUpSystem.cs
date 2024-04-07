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

    public void popUp(string playerType, string text)
    {
        Debug.Log("PopUpCalled");
        popUpBox = Instantiate(popUpBoxPrefab);
        PopUpPrefabController prefab = popUpBox.GetComponent<PopUpPrefabController>();
        prefab.printText(playerType, text);
    }
}
