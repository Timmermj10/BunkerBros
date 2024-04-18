using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
    private Text timer;
    float numSecondsPassed = 0;
    private int lastSecond;

    // Start is called before the first frame update
    void Start()
    {
        timer = GetComponent<Text>();
        numSecondsPassed = 0;

        EventBus.Subscribe<GameOverEvent>(_setFinalTime);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        numSecondsPassed += Time.deltaTime;
        if (timer != null)
        {
            // Calculate the current second
            int currentSecond = Mathf.FloorToInt(numSecondsPassed);


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
                timer.text = $"Time: {minutes}:{seconds}";

                //Update last second
                lastSecond = currentSecond;
            }
        }
    }

    private void _setFinalTime(GameOverEvent e)
    {
        PlayerPrefs.SetInt("time", lastSecond);
    }

}
