using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIOnDeath : MonoBehaviour
{
    // When the player is dead, remove the UI elements until they are respawned

    // Bool for if we are starting the game
    bool gameStart = true;

    // Bool for if we are respawning currently
    bool respawning = false;

    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<ItemUseEvent>(playerRespawn);
        EventBus.Subscribe<PlayerRespawnEvent>(playerRespawnComplete);
        //gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Check if there is a player
        if (GameObject.Find("player") == null && !respawning)
        {
            gameObject.SetActive(false);
        }
    }

    public void playerRespawn(ItemUseEvent e)
    {
        if (e.itemID == 9)
        {
            gameObject.SetActive(true);
            respawning = true;
        }
    }

    public void playerRespawnComplete(PlayerRespawnEvent e)
    {
        respawning = false;
    }
}
