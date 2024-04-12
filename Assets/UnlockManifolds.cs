using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class UnlockManifolds : MonoBehaviour
{
    // List of buttons
    public List<Button> buttons;

    public List<int> shuffledButtons;

    // Shuffled version of buttons
    public List<int> shuffledButtons1;
    public List<int> shuffledButtons2;
    public List<int> shuffledButtons3;

    // Keep track of how many buttons were pressed in sequence
    int counter = 0;

    private void Start()
    {
        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(RestartTheGame);
    }

    public void RestartTheGame(RadioTowerActivatedPlayerEvent e)
    {
        // Store the code
        shuffledButtons = e.code;

        // Reset the press counter
        counter = 0;

        // SHuffle the buttons with a random seed from 0-100
        for (int i = 0; i < 6; i++)
        {
            buttons[i].GetComponentInChildren<Text>().text = e.code[i].ToString();
        }

        for (int i = 1; i < 7; i++)
        {
            // Set the text of the buttons to correct number
            //shuffledButtons[i - 1].GetComponentInChildren<Text>().text = i.ToString();

            // Set all buttons to pressable
            // shuffledButtons[i - 1].interactable = true;
            buttons[i - 1].interactable = true;

            // Our initial color
            // shuffledButtons[i - 1].image.color = new Color32(177, 220, 233, 255);
            buttons[i - 1].image.color = new Color32(87, 87, 87, 255);
        }
    }

    public void pressButton(Button button)
    {
        if (int.Parse(button.GetComponentInChildren<Text>().text) - 1 == counter) 
        { 
            // Increment the counter
            counter++;

            // Make the button not interactable
            button.interactable = false;

            // Update the button color
            button.image.color = Color.green;

            // If we have pressed all the buttons correctly
            if (counter == 6)
            {
                // Present the result for winning
                StartCoroutine(presentResult(true));
            }
        }
        else
        {
            // Present the result for losing
            StartCoroutine(presentResult(false));
        }
    }

    public IEnumerator presentResult(bool win)
    {
        // If the player lost
        if (!win)
        {
            foreach(var button in buttons)
            {
                // Update the color of the button to red
                button.image.color = Color.red;

                // Make the button not interactable
                button.interactable = false;
            }

            // Wait for two seconds
            yield return new WaitForSeconds(2f);

            // Restart the game
            RestartTheGame(new RadioTowerActivatedPlayerEvent(shuffledButtons));
        }
        else
        {
            // Wait for one second
            yield return new WaitForSeconds(1f);

            // Publish a RadioTowerActivatedManagerEvent
            EventBus.Publish<RadioTowerActivatedManagerEvent>(new RadioTowerActivatedManagerEvent());
        }    
    }
}
