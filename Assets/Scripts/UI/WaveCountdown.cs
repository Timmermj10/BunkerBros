using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCountDown : MonoBehaviour
{
    // Float to hold the time variable
    float timer = 15f;

    // Update only on full seconds
    int lastSecond = 15;

    // Bool to hold if the timer has started
    bool timerStart = true;

    // UI element for the timer
    Text roundCountdown;

    // Wave manager instance
    WaveManager waveManager;

    // Start is called before the first frame update
    void Start()
    {
        // Listen for wave ended events
        EventBus.Subscribe<WaveEndedEvent>(_startCountdown);

        // Listen for wave started events
        EventBus.Subscribe<WaveStartedEvent>(_endCountdown);

        // Grab the round countdown
        roundCountdown = GameObject.Find("WaveCountdown").GetComponent<Text>();

        // Get reference to the wavemanager
        waveManager = GameObject.Find("WaveManager").GetComponent<WaveManager>();
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
        // Start the clock
        timerStart = true;
    }

    public void _endCountdown(WaveStartedEvent e)
    {
        // End the timer
        timerStart = false;

        // Reset the clock
        timer = 15f;

        // Update the text
        roundCountdown.text = "";
    }
}
