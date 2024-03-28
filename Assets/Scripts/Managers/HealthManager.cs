using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

public class HealthManager : MonoBehaviour
{
    //Shield
    private Shield towerShield;

    // Health
    private HasHealth playerHealth;
    public HasHealth towerHealth;

    // UI
    public Text playerHealthUI;
    public Text towerHealthUI;


    private void Start()
    {
        towerShield = GetComponent<Shield>();

        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);

        Transform target = GameObject.FindWithTag("Player").transform;

        while (target.parent != null)
        {
            target = target.parent;
            //Debug.Log(target.name);
        }

        playerHealth = target.GetComponent<HasHealth>();
        //Debug.Log(playerHealth.currentHealth);
    }

    void Update()
    {
        // Adjust values to min out at 1
        playerHealth.currentHealth = Mathf.Max(playerHealth.currentHealth, 0);
        towerHealth.currentHealth = Mathf.Max(towerHealth.currentHealth, 0);
    }

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        playerHealth = e.activePlayer.GetComponent<HasHealth>();
    }
}