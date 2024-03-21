using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class HoldPickup : MonoBehaviour
{
    // Time it will take to pick items up
    public float timeToPickup = 3.0f;

    // Whether the button is pressed down
    private bool buttonPressed = false;

    // Items that are in range to be picked up
    public List<GameObject> itemsInRange = new List<GameObject>();

    // Items that have been successfully picked up
    public List<GameObject> pickedUpItems = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // If we are pressing down the button and there is an item that can be pickedup
        if (buttonPressed && itemsInRange.Count > 0)
        {
            timeToPickup -= Time.deltaTime;
        }

        // If we aren't pressing down the button
        else if (timeToPickup != 3.0f)
        {
            timeToPickup = 3.0f;
        }

        // Check if we have reached the end of the timer
        if (timeToPickup <= 0)
        {
            timeToPickup = 3.0f;

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
                    else
                    {
                        // Publish a PickUpEvent
                        EventBus.Publish<PickUpEvent>(new PickUpEvent(pickup));
                    }

                    // Destroy the item
                    Destroy(item);
                }
            }

            // Clear the items in range list
            itemsInRange.Clear();
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
