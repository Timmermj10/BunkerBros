using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCountDown : MonoBehaviour
{
    // Float to hold the time variable
    float timer = 30f;

    // Update only on full seconds
    int lastSecond = 30;

    //The amount of time between waves
    private float timeBetweenRounds = 30f;

    // Bool to hold if the timer has started
    bool timerStart = false;

    // UI element for the timer
    Text roundCountdown;

    // Wave manager instance
    WaveManager waveManager;

    //Boolean for if we are in the tutorial
    private bool inTutorial = true;

    // Start is called before the first frame update
    void Start()
    {
        // Listen for wave ended events
        EventBus.Subscribe<WaveEndedEvent>(_startCountdown);

        // Listen for wave started events
        EventBus.Subscribe<WaveStartedEvent>(_endCountdown);

        // Listen for tutorial end events
        EventBus.Subscribe<TutorialEndedEvent>(_endTutorial);

        // Grab the round countdown
        roundCountdown = GameObject.Find("WaveCountdown").GetComponent<Text>();
        //Debug.Log($"roundCountdown is {roundCountdown}");
        roundCountdown.text = "";

        // Get reference to the wavemanager
        waveManager = GameObject.Find("GameManager").GetComponent<WaveManager>();
        //Debug.Log($"WaveManager is {waveManager}");

        timer = timeBetweenRounds;
        lastSecond = (int)timeBetweenRounds;
    }

    private void Update()
    {
        if (timerStart)
        {
            // Update elapsed time
            timer -= Time.deltaTime;

            // Calculate the current second
            int currentSecond = Mathf.FloorToInt(timer);

            //Check if a new second has started since the last update
            if (currentSecond != lastSecond)
            {
                // Update the UI
                roundCountdown.text = $"WAVE STARTING IN\n {currentSecond}";

                //Update last second
                lastSecond = currentSecond;
            }

            // If the countdown has reached zero
            if (currentSecond == 0)
            {
                waveManager.StartWave();
            }
        }
    }

    public void _startCountdown(WaveEndedEvent e)
    {

        if (!inTutorial)
        {
            // Start the clock
            timerStart = true;
            //Debug.Log("Starting wave countdown timer");
        }
    }

    public void _endCountdown(WaveStartedEvent e)
    {
        // End the timer
        timerStart = false;

        // Reset the clock
        timer = timeBetweenRounds;

        // Update the text
        roundCountdown.text = "";
    }

    private void _endTutorial(TutorialEndedEvent e)
    {
        inTutorial = false;
    }
}
