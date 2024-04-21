using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameWinUI : MonoBehaviour
{

    private Text winText;

    void Start()
    {
        winText = GetComponent<Text>();

        int totalSeconds = PlayerPrefs.GetInt("time");

        int minutes = (int)(totalSeconds / 60);

        int numSeconds = totalSeconds % 60;
        string seconds = numSeconds.ToString();
        if (numSeconds < 10)
        {
            seconds = "0" + seconds;
        }

        winText.text = $"CONGRATULATIONS! YOU ESCAPED IN {minutes}:{seconds}";
    }
}
