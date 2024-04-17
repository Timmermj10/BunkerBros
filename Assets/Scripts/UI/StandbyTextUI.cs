using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class StandbyTextUI : MonoBehaviour
{
    // Where we will update the text
    private Text standbyText;

    // Int to determine what section we are in
    private int section = 1;

    // Reference to buttonUpdate on the playerRespawn button
    ButtonUpdate playerRespawn;

    // Bool to hold if the button is selected
    private bool isSelected = false;

    private bool afterCarpetBombing = false;

    // Hold whether this is the first spawn
    private bool firstSpawn = true;

    // On start get reference to the text component
    private void Start()
    {
        // Get the text component of the standByText Text component
        standbyText = gameObject.GetComponent<Text>();

        // Get a reference to the ButtonUpdate script on the player respawn button
        if (GameObject.Find("PlayerRespawn") != null)
        {
            playerRespawn = GameObject.Find("PlayerRespawn").GetComponent<ButtonUpdate>();
        }

        // Subscribe to Manager Button Click Events
        EventBus.Subscribe<ManagerButtonClickEvent>(_ButtonClicked);
        EventBus.Subscribe<LastWaveOverEvent>(_disableRespawn);
    }

    private void _disableRespawn(LastWaveOverEvent e)
    {
        afterCarpetBombing = true;
    }

    void Update()
    {
        // If the player is dead and we aren't currently respawning
        if (GameObject.Find("player") == null && playerRespawn != null && !playerRespawn.respawning && !afterCarpetBombing)
        {
            // Check if the timer is up
            if (playerRespawn.timer > 0 && !firstSpawn)
            {
                // Update the text to tell the player that the manager is about to choose a location
                standbyText.text = "Prepare for deployment in\n" + string.Format("{0:F1}", playerRespawn.timer);
            }

            // Check if the manager has the redeploy selected
            else if (!isSelected)
            {
                if (section != 2)
                {
                    // Update the text to tell the player that the manager is about to choose a location
                    standbyText.text = "Waiting for Partner";

                    // Update section
                    section = 2;
                }
            }

            // Otherwise
            else
            {
                if (section != 3)
                {
                    // Update the text to tell the player that the manager is about to choose a location
                    standbyText.text = "Manager choosing position";

                    // Update section
                    section = 3;
                }
            }  
        } 
        // Reset the section when the player is respawned
        else
        {
            if (GameObject.Find("PlayerRespawn") != null)
            {
                playerRespawn = GameObject.Find("PlayerRespawn").GetComponent<ButtonUpdate>();
            }
            // Remove the text
            standbyText.text = "";

            // Update the value
            section = 1;

            if (firstSpawn)
            {
                firstSpawn = false;
            }
        }
    }

    public void _ButtonClicked(ManagerButtonClickEvent e)
    {
        // If the button clicked is player respawn
        if (e.button.gameObject.name == "PlayerRespawn")
        {
            isSelected = true;
        }

        // If we have the bool set to true
        else if (isSelected)
        {
            isSelected = false;
        }
    }
}
