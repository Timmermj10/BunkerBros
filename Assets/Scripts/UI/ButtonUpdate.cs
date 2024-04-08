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

    // Respawning player
    public bool respawning = false;

    // Player respawn timer
    public float timer = 5f;

    private bool isInTutorial = true;

    // Start is called before the first frame update
    void Start()
    {
        //Subscribe to the tutorial ended event
        EventBus.Subscribe<TutorialEndedEvent>(_tutorialEnded);

        // Get a reference to the button itself
        button = GetComponent<Button>();

        // Get a reference to the button info
        buttonInfo = GetComponent<ButtonInfo>();

        // Get a reference to the shop manager
        shopManager = GameObject.Find("GameManager").GetComponent<ShopManagerScript>();

        // Subscribe to Manager Button Click Events
        EventBus.Subscribe<ManagerButtonClickEvent>(_ButtonClicked);

        // Listen to respawning player events
        EventBus.Subscribe<PlayerRespawnEvent>(_respawningPlayer);

        // Listen to item used events
        EventBus.Subscribe<ItemUseEvent>(_checkRespawn);
    }

    // Update button colors and interactable status
    void Update()
    {
        if (!isInTutorial)
        {
            // Check if we have enough coins to purchase the item or if the player is dead for player respawn
            if (buttonInfo.itemID != 9)
            {
                if (shopManager.gold < shopManager.shopItems[buttonInfo.itemID].itemCost)
                {
                    button.interactable = false;
                }
                else
                {
                    button.interactable = true;
                }
            }
            // Dealing with player respawn
            else
            {
                // If the player is alive, disable the button
                if (GameObject.Find("player") != null && button.interactable == true)
                {
                    // Disable the button
                    button.interactable = false;
                }
                // If the timer has run out allow the manager to respawn the player
                else if (timer <= 0)
                {
                    // Enable the button
                    button.interactable = true;
                }
                // If the timer is not zero and the player is dead
                else if (GameObject.Find("player") == null && !respawning)
                {
                    // Decrease the timer
                    timer -= Time.deltaTime;
                }
            }
        }

            // If we have a currently selected Game Object, reset all the image colors to white
            if (EventSystem.current.currentSelectedGameObject)
            {
                GetComponent<Image>().color = Color.white;
            }
            if (shopManager.gold < shopManager.shopItems[buttonInfo.itemID].itemCost)
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

        // If the button clicked is this button 
        else if (button == e.button)
        {
            EventSystem.current.SetSelectedGameObject(e.button.gameObject);
            Debug.Log($"Setting Selected Gameobject to {EventSystem.current.currentSelectedGameObject}");
        }
    }

    // Run when a player is fully respawned
    public void _respawningPlayer(PlayerRespawnEvent e)
    {
        respawning = false;
    }

    // Run when a player respawn is purchased
    public void _checkRespawn(ItemUseEvent e)
    {
        if (e.itemID == 9)
        {
            // Set the respawning bool
            respawning = true;

            // If this button is the respawn
            if (buttonInfo.itemID == 9)
            {
                // Reset the timer
                timer = 5f;

                // Make the button not interactable
                button.interactable = false;

                // Make the image white
                button.gameObject.GetComponent<Image>().color = Color.white;
            }
        }
    }

    private void _tutorialEnded(TutorialEndedEvent e)
    {
        isInTutorial = false;
    }
}
