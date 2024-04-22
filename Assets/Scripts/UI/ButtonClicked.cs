using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClicked : MonoBehaviour
{

    private bool isWaveOver = false;
    public Button button;

    private void Start()
    {
        if (gameObject.name == "Evacuation")
        {
            button = GetComponent<Button>();
            EventBus.Subscribe<WaveEndedEvent>(waveIsOver);
            EventBus.Subscribe<WaveStartedEvent>(waveHasStarted);
        }
        
    }

    // Function for button click
    public void onButtonSelection()
    {
        // If the button that is selected is the evac button
        if (gameObject.name == "Evacuation")
        {
            Debug.Log("Evacuation Event");

            StartCoroutine(WaitForEndOfWave());
            Hide();
        }

        // Otherwise
        else
        {
            // Publish the managaer button click event
            EventBus.Publish<ManagerButtonClickEvent>(new ManagerButtonClickEvent(gameObject.GetComponent<Button>()));
        }
    }

    void Hide()
    {
        // Change the alpha value of the button's color to 0 (completely transparent)
        Color newColor = button.image.color;
        newColor.a = 0f;
        button.image.color = newColor;
    }

    private void waveIsOver(WaveEndedEvent e)
    {
        Debug.Log("Wave has ended");
        isWaveOver = true;
    }

    private void waveHasStarted(WaveStartedEvent e)
    {
        Debug.Log("Wave has started");
        isWaveOver = false;
    }

    private IEnumerator WaitForEndOfWave()
    {
        Debug.Log("Waiting For wave to be over");

        while (!isWaveOver)
        {
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Publishing Last Wave Event");
        EventBus.Publish<LastWaveEvent>(new LastWaveEvent());
        Destroy(gameObject);
        yield return null;
    }

}
