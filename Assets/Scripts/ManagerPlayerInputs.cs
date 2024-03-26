using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.Windows;

public class ManagerPlayerInputs : MonoBehaviour
{
    private Vector2 movementInputValue;

    PlayerInput playerInput;

    InputAction moveAction;

    private GameObject managerCamera;

    private InventoryUI inventory;

    public GameObject wallPrefab;

    private HashSet<Vector2> occupiedTiles = new HashSet<Vector2>();

    // Movement Speed
    public float movementSpeed = 5.0f;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        inventory = GameObject.Find("Inventory").GetComponent<InventoryUI>();

        managerCamera = GameObject.Find("ManagerCamera");

        MarkOccupiedTiles();

        EventBus.Subscribe<ObjectDestroyedEvent>(_UpdateOccupiedTiles);
    }

    private void MarkOccupiedTiles()
    {
        //Debug.Log($"Looking for occupied tiles");
        for(float i = -10; i < 10; i++)
        {
            for (float j = -10; j < 10; j++)
            {
                Vector3 rayStart = new Vector3(i, 10, j);
                Ray ray = new Ray(rayStart, Vector3.down);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~LayerMask.GetMask("Player")))
                {
                    Vector3 worldPosition = hit.point;

                    //Debug.Log($"tile {i}, {j} has height {worldPosition.y}");

                    if (worldPosition.y >= 1)
                    {
                        occupiedTiles.Add(new Vector2(Mathf.RoundToInt(i), Mathf.RoundToInt(j)));
                    }
                } else
                {
                    //Debug.Log($"Raycast for tile {i}, {j} hit nothing");
                }
            }
        }

        foreach (Vector2 tile in occupiedTiles)
        {
            //Debug.Log($"Occupied Tile = {tile}");
        }
    }

    private void _UpdateOccupiedTiles(ObjectDestroyedEvent e)
    {
        occupiedTiles.Remove(new Vector2(Mathf.RoundToInt(e.deathCoordinates.x), Mathf.RoundToInt(e.deathCoordinates.z)));
    }

    private void FixedUpdate()
    {
        // Set the velocity
        managerCamera.GetComponent<Rigidbody>().velocity = new Vector3(movementInputValue[0], 0, movementInputValue[1]) * movementSpeed;
    }


    private void OnMove(InputValue value)
    {
        // X,Z vector 2
        movementInputValue = value.Get<Vector2>();

        // Calculate the new position of the camera
        //Vector3 newPosition = new Vector3(managerCamera.position[0] + movementInputValue[0], managerCamera.position[1], managerCamera.position[2] + movementInputValue[1]);

        // Move the manager camera
        //managerCamera.position = newPosition;

        //Debug.Log("Manager Player: MovementInputValue = " + movementInputValue);
    }

    private void OnInteract(InputValue value)
    {

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Ray mouseRay = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenPointToRay(screenPosition);
        //Ray mouseRay = mainCamera.ScreenPointToRay(screenPosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~(LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player"))))
        {
            // Now worldPosition contains the 3D point in world space where the mouse is pointing
            Vector3 worldPosition = hit.point;
            Vector3 worldPositionRounded = new Vector3(Mathf.RoundToInt(worldPosition.x), worldPosition.y, Mathf.RoundToInt(worldPosition.z));
            // Debug.Log("Mouse is over the tile at Position: " + worldPositionRounded);


            // Check to see if that tile is within the camera area
            // Debug.Log($"X: {managerCamera.transform.position.x}, Z: {managerCamera.transform.position.z}");
            if (withinView(worldPositionRounded) && inventory.inventoryItems.Count > 0)
            {
                // Check to see if we have the Nuke Parts equipped in the inventory
                if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 0)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(0, itemUsedLocation, true)); // Changed to 0 for nuke parts
                }
                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 1 && !occupiedTiles.Contains(new Vector2(worldPositionRounded.x, worldPositionRounded.z)))
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

                    //Debug.Log("Publishing itemUseEvent for wall");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(1, itemUsedLocation, true)); // Changed to 1 for a wall
                }
                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 2 && !occupiedTiles.Contains(new Vector2(worldPositionRounded.x, worldPositionRounded.z)))
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

                    //Debug.Log("Publishing itemUseEvent for turret");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(2, itemUsedLocation, true)); // Changed to 2 for a turret
                }
                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 4)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    // Publish a use Event so the shop manager can update count and 
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(4, itemUsedLocation, true)); //id is 4 for nuke
                }
                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 5))
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    //Debug.Log("Publishing itemUseEvent for turret");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(5, itemUsedLocation, true)); // Changed to 2 for a missile
                }
            }
        }
        else
        {
            // Optional: Handle the case where the ray does not hit any collider
            //Debug.Log("Mouse is over nothing");
        }
    }

    private void OnCycle(InputValue value)
    {
        EventBus.Publish(new ManagerCycleEvent());
    }

    private bool withinView(Vector3 worldPosition)
    {
        float worldPositionX = worldPosition.x;
        float worldPositionZ = worldPosition.z;
        float managerPositionX = Mathf.RoundToInt(managerCamera.transform.position.x);
        float managerPositionZ = Mathf.RoundToInt(managerCamera.transform.position.z - 2);

       
        if ((managerPositionX - 6 <= worldPosition.x && worldPosition.x <= managerPositionX + 6)
            && (managerPositionZ - 6 <= worldPosition.z && worldPosition.z <= managerPositionZ + 6))
        {
            return true;
        }

        return false;
    } 
}


