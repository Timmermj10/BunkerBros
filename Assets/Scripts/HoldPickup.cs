using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HoldPickup : MonoBehaviour
{
    // Time it will take to pick items up
    public float timeToPickup = 3.0f;
    private float pickupTimer = 0;

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
            pickupTimer -= Time.deltaTime;
            Debug.Log($"Attempting to pickup, timer = {pickupTimer}");

            // Check if we have reached the end of the timer
            if (pickupTimer <= 0)
            {
                pickupTimer = timeToPickup;

                // For each item within range
                foreach (var item in itemsInRange)
                {
                    // If there is an item
                    if (item != null)
                    {
                        // Define a pickup InventoryItem
                        InventoryItem pickup = new InventoryItem();

                        // If the item is a goldchest
                        if (item.name is "GoldChest")
                        {
                            // Publish a CoinCollect event
                            EventBus.Publish<CoinCollect>(new CoinCollect(150));
                        }
                        else if (item.name is "MissileBox")
                        {
                            Debug.Log("Publishing missileparts pickup");
                            EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.MissileParts));
                        }

                        // Destroy the item
                        Destroy(item);
                    }
                }

                // Clear the items in range list
                itemsInRange.Clear();
            }
        }

        // If we aren't pressing down the button
        else if (pickupTimer != timeToPickup)
        {
            pickupTimer = timeToPickup;
        }
    }

    // When you walk up to a pickupable item
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag is "Pickup")
        {
            itemsInRange.Add(other.gameObject);
        }
    }

    // When you walk away from a pickupable item
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag is "Pickup")
        {
            itemsInRange.Remove(other.gameObject);
        }
    }

    void OnHoldToGet(InputValue value)
    {
        buttonPressed = value.isPressed;
    }
}
