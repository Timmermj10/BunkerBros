using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalWaveCountdown : MonoBehaviour
{
    // Float to hold the time variable
    float timer = 30f;

    // Update only on full seconds
    int lastSecond = 30;

    // Bool to hold if the timer has started
    bool timerStart = false;

    // UI element for the timer
    Text countdown;

    private float timeUntilExtraction = 5; //Should be 180

    // Start is called before the first frame update
    void Start()
    {
        // Listen for FinalWave events
        EventBus.Subscribe<LastWaveEvent>(_finalWave);

        // Grab the countdown
        countdown = GameObject.Find("FinalWaveCountdown").GetComponent<Text>();
        countdown.text = "";

        timer = timeUntilExtraction;
        lastSecond = (int)timeUntilExtraction;
    }

    private void _finalWave(LastWaveEvent e)
    {
        timer = timeUntilExtraction;
        timerStart = true;
    }

    private void Update()
    {
        if (timerStart)
        {
            // Update elapsed time
            timer -= Time.deltaTime;

            // Calculate the current second
            int currentSecond = Mathf.FloorToInt(timer);


            int minutes = (int)(currentSecond / 60);

            int numSeconds = currentSecond % 60;
            string seconds = numSeconds.ToString();
            if (numSeconds < 10)
            {
                seconds = "0" + seconds;
            }


            //Check if a new second has started since the last update
            if (currentSecond != lastSecond)
            {
                countdown.text = $"EXTRACTION TEAM IS ARRIVING IN\n {minutes}:{seconds}";

                //Update last second
                lastSecond = currentSecond;
            }

            // If the countdown has reached zero
            if (currentSecond == 0)
            {
                EventBus.Publish(new LastWaveOverEvent());
                timerStart = false;
                countdown.text = "";
                StartCoroutine(ExtractionTeamArrived());
            }
        }
    }

    private IEnumerator ExtractionTeamArrived()
    {

        for (int i = 0; i < 4; i++)
        {
            countdown.text = "";

            yield return new WaitForSeconds(0.4f);

            countdown.text = "Extraction Team has arrived!";

            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
}
