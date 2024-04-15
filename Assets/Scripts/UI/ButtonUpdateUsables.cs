using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUpdateUsables : MonoBehaviour
{
    // Button Colors
    Button button;

    // Button Info
    ButtonInfo buttonInfo;

    // Get reference to the list of silos
    [SerializeField]
    List<MissileSiloStatus> siloStatus;

    // Start is called before the first frame update
    void Start()
    {
        // Get a reference to the button itself
        button = GetComponent<Button>();

        // Make the button not interactable
        button.interactable =false;

        // Get a reference to the button info
        buttonInfo = GetComponent<ButtonInfo>();

        // Subscribe to Missle Silo loaded event
        EventBus.Subscribe<SiloLoadedEvent>(_SiloLoaded);

        // Subscribe to ItemUseEvents and listen for Nuke events
        EventBus.Subscribe<SiloUnloadedEvent>(_NukeLaunched);

        // Grab the silo status
        siloStatus = GameObject.Find("ManagerCamera").GetComponent<AirstrikeListener>().siloStatus;

        // Subscribe to Manager Button Click Events
        EventBus.Subscribe<ManagerButtonClickEvent>(_ButtonClicked);
    }

    // Update is called once per frame
    void Update()
    {
        if (siloStatus.Count > 0)
        {
            button.interactable = true;
            button.GetComponent<Image>().color = Color.white;
        }
    }

    // Update the button for Nukes if a Silo has been loaded
    public void _SiloLoaded(SiloLoadedEvent e)
    {
        // If we are working with the nuke button
        if (buttonInfo.itemID == 4) 
        {
            button.interactable = true;

            //Make the nuke botton selectable
            button.GetComponent<Image>().color = Color.white;
        }
    }

    // Update the button for Nukes if a nuke was launched
    public void _NukeLaunched(SiloUnloadedEvent e)
    {
        // If we are working with the nuke button
        if (buttonInfo.itemID == 4)
        {
            // Check if we have another nuke
            if (siloStatus.Count > 0)
            {
                // If we have another nuke

                // Update color to yellow
                button.GetComponent<Image>().color = Color.yellow;

                // Update most recently used
                ManagerPlayerInputsNew.mostRecentItem = button.gameObject;
            }
            // If we don't have another nuke
            else
            {
                // Update the color of the button
                button.GetComponent<Image>().color = Color.white;

                // Update the interactablity of the button
                button.interactable = false;

                // Make most recently used item null
                ManagerPlayerInputsNew.mostRecentItem = null;
            }
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
        else if (button == e.button)
        {
            EventSystem.current.SetSelectedGameObject(e.button.gameObject);
            //Debug.Log($"Setting Selected Gameobject to {EventSystem.current.currentSelectedGameObject}");
        }
    }
}
