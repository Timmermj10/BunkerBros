using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using UnityEngineInternal;
using System.Net.NetworkInformation;

public class ManagerPlayerInputsNew : MonoBehaviour
{
    //private KBMandController kb;
    private InputAction zoom;

    private Vector2 movementInputValue;
    private Vector2 zoomScroll;
    public float zoomSpeed = 1;
    private float localScale = 0;
    public float zoomScale = 2;
    private float originalSize;


    //Maximum distance you can respawn the player form the objective
    public static int maxRespawnDistanceFromObjective = 8;

    private bool canPlaceMultipleItemsInARow = false;
    private bool inTutorial = true;
    private Vector3 tutorialBoulderPosition = new Vector3(-11, 1.5f, -2);

    PlayerInput playerInput;

    InputAction moveAction;

    public static GameObject managerCamera;
    public GameObject pingCamera;
    private float pingOriginalSize;

    private InventoryUI inventory;

    public GameObject wallPrefab;

    // Reference to the ShopManager
    private ShopManagerScript shopManagerScript;

    // Movement Speed
    [SerializeField]
    public float movementSpeed = 10.0f;

    // Most recently used item
    static public GameObject mostRecentItem;

    // Public int to hold how many blocks away from center (will be used when we implement zoom)
    public static int blockCount = 5;
    public int origBlockCount;

    private PingManager pingManager;
    public float minX, maxX, minY, MaxY;

    // Bool to hold if we are currently doing the minigame
    bool minigame = false;

    //private void Awake()
    //{
    //    kb = new KBMandController();
    //    zoom = kb.ManagerPlayer.Zoom;
    //    zoom.performed += ScrollZoom;
    //}

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        origBlockCount = blockCount;
        //zoomSpeed = 1;
        zoom = playerInput.actions.FindAction("Zoom");
        moveAction = playerInput.actions.FindAction("Move");
        if (GameObject.Find("Inventory") != null)
        {
            inventory = GameObject.Find("Inventory").GetComponent<InventoryUI>();
        }

        //Subscribe to the tutorial ended event to change canPlaceMultipleItemsInARow and InTutorial
        EventBus.Subscribe<FirstTutorialWaveEvent>(_tutorialDefense);

        managerCamera = GameObject.Find("ManagerCamera");
        
        originalSize = managerCamera.GetComponent<Camera>().orthographicSize;
        pingOriginalSize = pingCamera.GetComponent<Camera>().orthographicSize;

        // Get reference to the ShopManagerScript
        shopManagerScript = GameObject.Find("GameManager").GetComponent<ShopManagerScript>();
        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();

        // Subscribe to the minigame events
        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(setMinigameTrue);
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(setMinigameFalse);
        EventBus.Subscribe<miniGameAbortEvent>(setMinigameTrueAbort);
    }


    private void FixedUpdate()
    {
        movementSpeed = 10 * zoomSpeed;
        // Set the size
        managerCamera.GetComponent<Camera>().orthographicSize = originalSize + localScale;
        pingCamera.GetComponent<Camera>().orthographicSize = pingOriginalSize + localScale;
        blockCount = origBlockCount + Mathf.RoundToInt(localScale);
        //Debug.Log("BlockCount is " + blockCount.ToString());
        // Set the velocity
        //if (localScale == originalSize)
        //{
        //    zoomSpeed = 1;
        //}
        Vector3 velo = new Vector3(movementInputValue[0], 0, movementInputValue[1]) * movementSpeed * Mathf.Max(.5f, (2.5f - zoomSpeed));
        Vector3 move = velo * Time.deltaTime;
        Vector3 pos = transform.position += move;
        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.z = Mathf.Clamp(pos.z, minY, MaxY);
        pos.y = 20;
        transform.position = pos;

        //Debug.Log("Zoom Max is " + Mathf.Max(.1f, (2f - zoomSpeed)));

        //CHEAT FOR SKIPPING TO FINAL WAVE
        if (Input.GetKeyDown(KeyCode.P))
        {
            EventBus.Publish(new RadioTowerActivatedManagerEvent());
            EventBus.Publish(new RadioTowerActivatedManagerEvent());
            EventBus.Publish(new RadioTowerActivatedManagerEvent());
            EventBus.Publish(new RadioTowerActivatedManagerEvent());
        }

    }

    private void OnZoom(InputValue value)
    {
        zoomScroll = -1 * value.Get<Vector2>();
        
        //Debug.Log("Zoom is" + zoomScroll.ToString());
        //Up scroll is *, -1
        //Down scroll is *, +1
        if (zoomScroll.y < 0) //If scrolling up, zoom in
        {
            localScale = localScale - 0.1f;
            zoomSpeed -= .1f;
            if (localScale <= zoomScale * -1)
            {
                zoomSpeed = .5f;
                localScale = zoomScale * -1;
            }
        } else if (zoomScroll.y > 0) //Scrolling down, zoom out
        {
            localScale += .1f;
            zoomSpeed += .1f;
            if (localScale >= zoomScale)
            {
                zoomSpeed = 2;
                localScale = zoomScale;
            }
        }
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

    //private void OnInteract(InputValue value)
    //{

    //    Vector2 screenPosition = Mouse.current.position.ReadValue();
    //    Ray mouseRay = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenPointToRay(screenPosition);
    //    //Ray mouseRay = mainCamera.ScreenPointToRay(screenPosition);
    //    RaycastHit hit;

    //    if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, ~(LayerMask.GetMask("Enemy") | LayerMask.GetMask("Player"))))
    //    {
    //        // Now worldPosition contains the 3D point in world space where the mouse is pointing
    //        Vector3 worldPosition = hit.point;
    //        Vector3 worldPositionRounded = new Vector3(Mathf.RoundToInt(worldPosition.x), worldPosition.y, Mathf.RoundToInt(worldPosition.z));
    //        // Debug.Log("Mouse is over the tile at Position: " + worldPositionRounded);


    //        // Check to see if that tile is within the camera area
    //        // Debug.Log($"X: {managerCamera.transform.position.x}, Z: {managerCamera.transform.position.z}");
    //        if (inventory != null)
    //        {
    //            if (withinView(worldPositionRounded) && inventory.inventoryItems.Count > 0)
    //            {
    //                // Check to see if we have the Nuke Parts equipped in the inventory
    //                if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 0)
    //                {
    //                    //get the location of the item
    //                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
    //                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

    //                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(0, itemUsedLocation, true)); // Changed to 0 for nuke parts
    //                }
    //                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 1 && !occupiedTiles.Contains(new Vector2(worldPositionRounded.x, worldPositionRounded.z)))
    //                {
    //                    //get the location of the item
    //                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
    //                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

    //                    //Debug.Log("Publishing itemUseEvent for wall");
    //                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(1, itemUsedLocation, true)); // Changed to 1 for a wall
    //                }
    //                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 2 && !occupiedTiles.Contains(new Vector2(worldPositionRounded.x, worldPositionRounded.z)))
    //                {
    //                    //get the location of the item
    //                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);
    //                    occupiedTiles.Add(new Vector2(worldPositionRounded.x, worldPositionRounded.z));

    //                    //Debug.Log("Publishing itemUseEvent for turret");
    //                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(2, itemUsedLocation, true)); // Changed to 2 for a turret
    //                }
    //                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 4)
    //                {
    //                    //get the location of the item
    //                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

    //                    // Publish a use Event so the shop manager can update count and 
    //                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(4, itemUsedLocation, true)); //id is 4 for nuke
    //                }
    //                else if (inventory.inventoryItems[inventory.inventoryItemsIndex] == 5)
    //                {
    //                    //get the location of the item
    //                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

    //                    //Debug.Log("Publishing itemUseEvent for turret");
    //                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(5, itemUsedLocation, true)); // Changed to 2 for a missile
    //                }
    //            }
    //        }
    //    }
    //    else
    //    {
    //        // Optional: Handle the case where the ray does not hit any collider
    //        //Debug.Log("Mouse is over nothing");
    //    }
    //}

    // NEW OnInteract Function for the new manager system
    private void OnInteract(InputValue value)
    {

        Vector2 screenPosition = Mouse.current.position.ReadValue();
        RaycastHit hit;

        //Get the position to raycast from
        Vector3 raycastPosition = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
        raycastPosition.y = 10f;

        if (Physics.Raycast(raycastPosition, Vector3.down, out hit, Mathf.Infinity, ~LayerMask.GetMask("Enemy", "Player", "Pickup")) && hit.collider.gameObject.layer == LayerMask.NameToLayer("Default")) 
        {
            // Now worldPosition contains the 3D point in world space where the mouse is pointing
            Vector3 worldPosition = hit.point;
            Vector3 worldPositionRounded = new Vector3(Mathf.RoundToInt(worldPosition.x), worldPosition.y, Mathf.RoundToInt(worldPosition.z));
            // Debug.Log("Mouse is over the tile at Position: " + worldPositionRounded);

            // THIS IS HOW YOU DETECT WHAT BUTTON IS SELECTED
            // THIS WILL BE USEFUL FOR PLACING THE OBJECTS AND SHOWING THE PREVIEW TO THE MANAGER

            // EventSystem.current holds a reference to the current event system
            GameObject selectedObj = EventSystem.current.currentSelectedGameObject;
            


            // If there isn't a selected object, 
            if (selectedObj == null && canPlaceMultipleItemsInARow)
            {
                // Set the selected object to most recently used
                selectedObj = mostRecentItem;
            }
            //Debug.Log($"Selected Gameobject is {selectedObj} and canPlaceMultipleItemsInARow = {canPlaceMultipleItemsInARow}");

            if (selectedObj != null && withinView(worldPositionRounded) && !minigame)
            {
                // Do something with the selected object

                // The player will have enough money to use this object
                // No need to check that, just send out an update to the coins based on the cost
                // Check if they can place another one after the update to coins, if not
                // If they can, set the current selectedObj, back to selected so they can spam place

                // Get the itemID of the item used
                int itemID = selectedObj.GetComponent<ButtonInfo>().itemID;

                // Publish the ItemUseEvent
                if (itemID == 0)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(0, itemUsedLocation, true)); // Changed to 0 for nuke parts
                }
                else if (itemID == 1)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    //Debug.Log("Publishing itemUseEvent for wall");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(1, itemUsedLocation, true)); // Changed to 1 for a wall
                }
                else if (itemID == 2)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    //Debug.Log("Publishing itemUseEvent for turret");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(2, itemUsedLocation, true)); // Changed to 2 for a turret
                }
                else if (itemID == 3)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    //Debug.Log("Publishing itemUseEvent for turret");
                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(3, itemUsedLocation, true)); // Changed to 3 for a gun
                }
                else if (itemID == 4)
                {

                    Debug.Log($"InTutorial = {inTutorial} and distance = {Vector3.Distance(worldPositionRounded, tutorialBoulderPosition)}");
                    if ((inTutorial && Vector3.Distance(worldPositionRounded, tutorialBoulderPosition) < 5) || !inTutorial)
                    {
                        //get the location of the item
                        Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                        // Publish a use Event so the shop manager can update count
                        EventBus.Publish<ItemUseEvent>(new ItemUseEvent(4, itemUsedLocation, true)); //id is 4 for nuke
                    }
                }
                else if (itemID == 5)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(5, itemUsedLocation, true)); // Changed to 5 for a missile
                } else if (itemID == 6)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(6, itemUsedLocation, true)); // Changed to 6 for a HealthPack
                } else if (itemID == 7)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(7, itemUsedLocation, true)); // Changed to 7 for a RepairKit
                } else if (itemID == 8)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(8, itemUsedLocation, true)); // Changed to 8 for a AmmoCrate
                }
                else if (itemID == 9 && Vector3.Distance(worldPositionRounded, new Vector3(0, 1, 0)) < maxRespawnDistanceFromObjective)
                {
                    //get the location of the item
                    Vector3 itemUsedLocation = new Vector3(worldPositionRounded.x, worldPositionRounded.y + 0.5f, worldPositionRounded.z);

                    EventBus.Publish<ItemUseEvent>(new ItemUseEvent(9, itemUsedLocation, true)); // Changed to 9 for a playerRespawn
                }


                if (selectedObj.transform.parent == GameObject.Find("Purchasables").transform)
                {
                    // Cost of item
                    float cost = shopManagerScript.shopItems[itemID].itemCost;

                    // Update coin count
                    //shopManagerScript.coins -= cost;
                    EventBus.Publish(new CoinCollect((int)-cost));

                    // Check to see if they can purchase another one and it's not onetime purchase
                    if (selectedObj.GetComponent<ButtonInfo>().itemID == 3)
                    {
                        // If they purchased the gun
                        // Send out the purchase event for jeremy's implementation
                        // EventBus.Publish<PurchaseEvent>(new PurchaseEvent(shopManagerScript.shopItems[3]));

                        Destroy(selectedObj);
                    }
                    else if (shopManagerScript.gold >= cost)
                    {
                        // Most recently used item
                        mostRecentItem = selectedObj;

                        // Set the color to the selected color
                        selectedObj.GetComponent<Image>().color = Color.yellow;
                    }
                    // Otherwise remove the previous object
                    else
                    {
                        mostRecentItem = null;
                    }
                }
            }
        }
    }

    private void OnCycle(InputValue value)
    {
        EventBus.Publish(new ManagerCycleEvent());
    }

    private void OnPing(InputValue value)
    {
        Vector2 screenPosition = Mouse.current.position.ReadValue();
        Vector3 raycastPosition = GameObject.Find("ManagerCamera").GetComponent<Camera>().ScreenToWorldPoint(screenPosition);
        RaycastHit hit;
        if (Physics.Raycast(raycastPosition, Vector3.down, out hit, Mathf.Infinity))
        {
            HasPing hasPing = hit.transform.gameObject.GetComponent<HasPing>();
            if (hasPing)
                hasPing.TogglePing();
            else
                pingManager.ManagerPing(hit.point);
        }
    }

    public static bool withinView(Vector3 worldPosition)
    {
        float vw = managerCamera.GetComponent<Camera>().orthographicSize;
        float vh = managerCamera.GetComponent<Camera>().orthographicSize;
        Vector3 offset = worldPosition - managerCamera.transform.position;

        Vector3 clamped = new(Mathf.Clamp(offset.x, -vw, vw), offset.y, Mathf.Clamp(offset.z, -vh, vh));
        if (clamped == offset)
        {
            return true;
        }

        return false;

        //float worldPositionX = worldPosition.x;
        //float worldPositionZ = worldPosition.z;
        //float managerPositionX = Mathf.RoundToInt(managerCamera.transform.position.x);
        //float managerPositionZ = Mathf.RoundToInt(managerCamera.transform.position.z);


        //if ((managerPositionX - blockCount <= worldPosition.x && worldPosition.x <= managerPositionX + blockCount)
        //    && (managerPositionZ - blockCount <= worldPosition.z && worldPosition.z <= managerPositionZ + blockCount))
        //{
        //    return true;
        //}

        //return false;
    }

    private void _tutorialDefense(FirstTutorialWaveEvent e)
    {
        canPlaceMultipleItemsInARow = true;
        inTutorial = false;
    }

    public void setMinigameTrue(RadioTowerActivatedPlayerEvent e)
    {
        minigame = true;
    }

    public void setMinigameFalse(RadioTowerActivatedManagerEvent e)
    {
        minigame = false;
    }

    public void setMinigameTrueAbort(miniGameAbortEvent e)
    {
        minigame = true;
    }
}

