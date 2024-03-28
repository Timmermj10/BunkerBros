using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUpdate : MonoBehaviour
{
    // Button Colors
    Button button;

    // Button Info
    ButtonInfo buttonInfo;

    // Shop Manager
    private ShopManagerScript shopManager;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the button itself
        button = GetComponent<Button>();

        // Get a reference to the button info
        buttonInfo = GetComponent<ButtonInfo>();

        // Get a reference to the shop manager
        shopManager = GameObject.Find("ShopManager").GetComponent<ShopManagerScript>();

    }

    // Update is called once per frame
    void Update()
    {
        // Check if we have enough coins to purchase the item
        if (shopManager.coins < shopManager.shopItems[buttonInfo.itemID].itemCost)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }

        // If we have a currently selected Game Object, reset all the image colors to white
        if (EventSystem.current.currentSelectedGameObject)
        {
            GetComponent<Image>().color = Color.white;
        }
        if (shopManager.coins <= shopManager.shopItems[buttonInfo.itemID].itemCost)
        {
            GetComponent<Image>().color = Color.white;
        }

        // THIS IS HOW YOU DETECT WHAT BUTTON IS SELECTED
        // THIS WILL BE USEFUL FOR PLACING THE OBJECTS AND SHOWING THE PREVIEW TO THE MANAGER

        // EventSystem.current holds a reference to the current event system
        //GameObject selectedObj = EventSystem.current.currentSelectedGameObject;

        //if (selectedObj != null)
        //{
        //    Do something with the selected object
        //    Debug.Log("Currently selected button: " + selectedObj.name);
        //}
    }
}
