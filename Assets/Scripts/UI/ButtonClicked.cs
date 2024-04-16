using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour
{
    // Function for button click
    public void onButtonSelection()
    {
        // If the button that is selected is the evac button
        if (gameObject.name == "Evacuation")
        {
            Debug.Log("Evacuation Event");
            // Publish the lave wave event
            EventBus.Publish<WaveEndedEvent>(new WaveEndedEvent());
            EventBus.Publish<LastWaveEvent>(new LastWaveEvent());

            // Delete the button
            Destroy(gameObject);
        }

        // Otherwise
        else
        {
            // Publish the managaer button click event
            EventBus.Publish<ManagerButtonClickEvent>(new ManagerButtonClickEvent(gameObject.GetComponent<Button>()));
        }
    }
}
