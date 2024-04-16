using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoundCounterUI : MonoBehaviour
{
    // Round count
    private int roundCount = 1;

    // Manager Round Text
    private Text managerRounds;

    // Player Round Text
    private Text playerRounds;

    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to round start events
        EventBus.Subscribe<WaveEndedEvent>(_UpdateRoundUI);

        // Get reference to the text for rounds
        managerRounds = GameObject.Find("ManagerRoundCounter").GetComponent<Text>();
        playerRounds = GameObject.Find("PlayerRoundCounter").GetComponent<Text>();
    }

    public void _UpdateRoundUI(WaveEndedEvent e)
    {
        // Increment the round count
        roundCount++;

        // Update the UI
        managerRounds.text = $"Round: {roundCount}";
        playerRounds.text = $"Round: {roundCount}";
    }
}
