using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.Loading;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInteract : MonoBehaviour
{
    // Time it will take to pick items up
    public float timeToInteract = 2.0f;
    private float interactTimer = 0;

    // Whether the button is pressed down
    private bool buttonPressed = false;

    // Items that are in range to be picked up
    public List<GameObject> itemsInRange = new List<GameObject>();

    // Items that have been successfully picked up
    public List<GameObject> pickedUpItems = new List<GameObject>();


    // Update is called once per frame
    private void FixedUpdate()
    {
        // If we are pressing down the button and there is an item that can be pickedup
        if (buttonPressed && itemsInRange.Count > 0)
        {
            EventBus.Publish(new InteractTimerStartedEvent(timeToInteract));

            interactTimer -= Time.deltaTime;
            //Debug.Log($"Attempting to interact, timer = {interactTimer}");

            // Check if we have reached the end of the timer
            if (interactTimer <= 0)
            {
                interactTimer = timeToInteract;
                EventBus.Publish(new InteractTimerEndedEvent());

                // For each item within range
                foreach (var item in itemsInRange)
                {
                    // If there is an item
                    if (item != null && item.tag is "Pickup")
                    {
                        // Define a pickup InventoryItem
                        InventoryItem pickup = new InventoryItem();

                        // If the item is a goldchest
                        if (item.name is "GoldChest" || item.name is "ColdChest(Clone)")
                        {
                            // Publish a CoinCollect event
                            EventBus.Publish<CoinCollect>(new CoinCollect(150));
                        }
                        else if (item.name is "MissileBox" || item.name is "MissileBox(Clone)")
                        {
                            //Debug.Log("Publishing missileparts pickup");
                            EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.MissileParts));
                        }

                        // Destroy the item
                        EventBus.Publish(new ObjectDestroyedEvent(item.name, item.tag, item.transform.position));
                        Destroy(item);
                    }
                    else if (item != null && item.tag is "Interactable")
                    {
                        if (item.name is "MissileSilo" || item.name is "MissileSilo(Clone)")
                        {
                            MissileSiloStatus silo = item.GetComponent<MissileSiloStatus>();
                            //Debug.Log("Attempting to load Silo");
                            if (silo != null && !silo.isSiloLoaded() && GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.MissileParts))
                            {
                                //Debug.Log("Loading MissileSilo");
                                silo.loadSilo();
                            }
                            else
                            {
                                //Debug.Log($"Failed to load Silo: silo status = {silo.isSiloLoaded()}, doesThePlayerHaveMissileParts = {GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.MissileParts)}");
                            }
                            
                        }
                   
                    }
                }

                // Clear the items in range list
                itemsInRange.Clear();
            }
        }

        // If we aren't pressing down the button
        else if (interactTimer != timeToInteract)
        {
            interactTimer = timeToInteract;
            EventBus.Publish(new InteractTimerEndedEvent());
        }
    }

    // When you walk up to a pickupable item
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Pickup" || other.gameObject.tag is "Interactable")
        {
            itemsInRange.Add(other.gameObject);
        }
    }

    // When you walk away from a pickupable item
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag is "Pickup" || other.gameObject.tag is "Interactable")
        {
            itemsInRange.Remove(other.gameObject);
        }
    }

    void OnHoldToGet(InputValue value)
    {
        buttonPressed = value.isPressed;
    }
}
