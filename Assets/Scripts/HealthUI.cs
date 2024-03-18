using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    // Health
    public HasHealth playerHealth;
    public HasHealth towerHealth;

    // UI
    public Text playerHealthUI;
    public Text towerHealthUI;

    // Update is called once per frame
    void Update()
    {
        // Adjust values to min out at 1
        playerHealth.currentHealth = Mathf.Max(playerHealth.currentHealth, 0);
        towerHealth.currentHealth = Mathf.Max(towerHealth.currentHealth, 0);

        // Set UI
        playerHealthUI.text = $"Player Health: {playerHealth.currentHealth}";
        towerHealthUI.text = $"Tower Health: {towerHealth.currentHealth}";
    }
}
