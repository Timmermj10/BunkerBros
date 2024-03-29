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

        // Subscribe to Manager Button Click Events
        EventBus.Subscribe<ManagerButtonClickEvent>(_ButtonClicked);
    }

    // Update button colors and interactable status
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
        if (shopManager.coins < shopManager.shopItems[buttonInfo.itemID].itemCost)
        {
            GetComponent<Image>().color = Color.white;
        }
    }

    // Run when a button is clicked
    public void _ButtonClicked(ManagerButtonClickEvent e)
    {
        // If the button clicked is not this button and it was the most recently used
        if (button.gameObject == ManagerPlayerInputsNew.mostRecentItem && button != e.button)
        {
            // Update most recently used
            ManagerPlayerInputsNew.mostRecentItem = e.button.gameObject;

            // Update the color of this button
            button.GetComponent<Image>().color = Color.white;
        }
    }
}
