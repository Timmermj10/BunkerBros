using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileSelect : MonoBehaviour
{
    private InventoryUI inventory;
    private void Start()
    {
        inventory = GameObject.Find("Inventory").GetComponent<InventoryUI>();
    }


    // Update is called once per frame
    void Update()
    {
        // Check for a left mouse button click
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            // Create a ray from the mouse cursor on screen in the direction of the camera
            Ray ray = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPosition = hit.point;

                // Rounded worldPosition
                Vector3 worldPositionRounded = new Vector3(Mathf.RoundToInt(worldPosition.x), 0f, Mathf.RoundToInt(worldPosition.z));

                // Check to see if we have the Airstrike equipped in the inventory
                if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 0)
                {
                    // Publish the airstrike event
                    EventBus.Publish<AirstrikeEvent>(new AirstrikeEvent(worldPositionRounded));

                    // Publish a use Event so the shop manager can update count
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(0));
                }

                Debug.Log("Mouse is over the tile at Position: " + new Vector3(Mathf.RoundToInt(worldPosition.x), 0f, Mathf.RoundToInt(worldPosition.z)));
            }
            else
            {
                // Optional: Handle the case where the ray does not hit any collider
                Debug.Log("Mouse is over nothing");
            }
        }
    }
}
