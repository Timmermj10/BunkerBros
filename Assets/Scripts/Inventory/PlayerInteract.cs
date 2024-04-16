using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;

public class PlayerInteract : MonoBehaviour
{
    // Time it will take to pick items up
    public float timeToInteract = 2.0f;
    private float interactTimer = 2.0f;

    // Whether the button is pressed down
    public static bool buttonPressed = false;

    // Reference to shop manager script
    private ShopManagerScript shopManagerScript;

    // Items that are in range to be picked up
    [SerializeField]
    private List<GameObject> itemsInRange = new List<GameObject>();

    //Reference to player
    GameObject player;

    // Is there a radio tower currently being activated
    private bool radioTowerInteract = false;


    [Header("Text Popups for Interactables")]
    // Camera
    private Camera playerCam;
    public float distance = 3f;
    public LayerMask mask;
    private PlayerUI playerUI;


    private void Start()
    {
        playerUI = GameObject.Find("player").GetComponent<PlayerUI>();
        playerCam = GameObject.Find("PlayerCamera").GetComponent<Camera>();
        shopManagerScript = GameObject.Find("GameManager").GetComponent<ShopManagerScript>();

        player = GameObject.Find("player");

        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(playerStartedTower);
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(managerEndedTower);
        EventBus.Subscribe<miniGameAbortEvent>(managerEndedTowerAbort);
    }

    private void Update()
    {
        playerUI.UpdateText(string.Empty);
        // Create a ray at the center of the camera, shooting outwards
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        Debug.DrawRay(ray.origin, ray.direction * distance);

        RaycastHit hitInfo; // Variable to hold collision info
        if (Physics.Raycast(ray, out hitInfo, distance, mask))
        {
            if (hitInfo.collider.GetComponent<Interactable>() != null)
            {
                playerUI.UpdateText(hitInfo.collider.GetComponent<Interactable>().promptMessage);
            }
        }
    }


    // Update is called once per frame
    private void FixedUpdate()
    {

        // If we are pressing down the button and there is an item that can be pickedup
        if (buttonPressed && itemsInRange.Count > 0)
        {
            GameObject item = itemsInRange[0];

            //If the timer not already going, start it
            if (interactTimer >= timeToInteract)
            {
                if (itemsInRange.Count > 0 && canInteractWithObject(item))
                {
                    EventBus.Publish(new InteractTimerStartedEvent(timeToInteract));
                    //Debug.Log("Starting Timer");
                }
                else
                {
                    itemsInRange.Remove(item);
                    //Debug.Log($"removing item to range. count = {itemsInRange.Count}");
                    EventBus.Publish(new itemRemovedFromPickupRangeEvent(itemsInRange.Count));
                }
            }

            //Decrease interact timer
            interactTimer -= Time.deltaTime;

            // Check if we have reached the end of the timer
            if (interactTimer <= 0)
            {
                interactTimer = timeToInteract;
                EventBus.Publish(new InteractTimerEndedEvent());

                // If there is an item
                if (item != null && item.tag is "Pickup")
                {
                    // Define a pickup InventoryItem
                    InventoryItem pickup = new InventoryItem();

                    // If the item is a goldchest
                    if (item.name is "ChestPack" || item.name is "ChestPack(Clone)")
                    {
                        // Publish a CoinCollect event
                        EventBus.Publish<CoinCollect>(new CoinCollect(150));
                    }
                    else if (item.name is "NukeCrate" || item.name is "NukeCrate(Clone)")
                    {
                        //Debug.Log("Publishing NukeParts pickup");
                        EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.NukeParts));
                    }
                    //TODO: add ammo pickup
                    else if (item.name is "RepairKit" || item.name is "RepairKit(Clone)")
                    {
                        EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.RepairKit));
                    }
                    else if (item.name is "HealthPack" || item.name is "HealthPack(Clone)")
                    {
                        EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.HealthPack));
                    }
                    else if (item.name is "AmmoCrate" || item.name is "AmmoCrate(Clone)")
                    {
                        EventBus.Publish<PickUpEvent>(new PickUpEvent(ActivePlayerInventory.activePlayerItems.AmmoKit));
                    }
                    else if (item.name is "GunCrate" || item.name is "GunCrate(Clone)")
                    {
                        EventBus.Publish<PurchaseEvent>(new PurchaseEvent(shopManagerScript.shopItems[3]));
                    }

                    // Destroy the item
                    EventBus.Publish(new ObjectDestroyedEvent(item.name, item.tag, item.transform.position));
                    itemsInRange.Remove(item);
                    Destroy(item);
                    //Let the timer know the count has been updated
                    //Debug.Log($"Publishing itemRemovedEvent with count = {itemsInRange.Count}");
                    EventBus.Publish(new itemRemovedFromPickupRangeEvent(itemsInRange.Count));
                }
                else if (item != null && item.tag is "Interactable")
                {
                    if ((item.transform.parent.name is "MissileSilo" || item.transform.parent.name is "MissileSilo(Clone)") && !item.transform.parent.GetComponent<MissileSiloStatus>().isSiloLoaded() && player.GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.NukeParts))
                    {
                        ChangeMaterial changeMaterialScript = item.transform.parent.GetComponent<ChangeMaterial>();
                        bool enable = true;
                        if (changeMaterialScript != null)
                        {
                            changeMaterialScript.ChangeKnobMaterial(item.transform.parent.gameObject, enable);
                        }
                        else
                        {
                            Debug.LogWarning("ChangeMaterial script not found on item: " + item.name);
                        }
                        // Make it so you can not use the same silo until it is unloaded
                        // item.tag = "Untagged";

                        MissileSiloStatus silo = item.transform.parent.GetComponent<MissileSiloStatus>();
                        //Debug.Log("Loading Silo");
                        if (silo != null && !silo.isSiloLoaded())
                        {
                            //Debug.Log("Loading MissileSilo");
                            silo.loadSilo();

                            // Publish Silo Loaded Event
                            //Debug.Log("Publishing Event");
                            EventBus.Publish<SiloLoadedEvent>(new SiloLoadedEvent(silo));

                            //Take the parts out of the player inventory
                            ActivePlayerInventory inventory = player.GetComponent<ActivePlayerInventory>();
                            inventory.useItem(ActivePlayerInventory.activePlayerItems.NukeParts);
                        }
                    }
                    else if (item.transform.parent.name is "RadioTower" || item.transform.parent.name is "RadioTower(Clone)")
                    {
                        ChangeMaterial changeMaterialScript = item.transform.parent.GetComponent<ChangeMaterial>();
                        bool enable = true;
                        if (changeMaterialScript != null)
                        {
                            changeMaterialScript.ChangeKnobMaterial(item.transform.parent.gameObject, enable);
                        }
                        else
                        {
                            Debug.LogWarning("ChangeMaterial script not found on item: " + item.name);
                        }
                        // Make it so you can not use the same radio tower
                        item.tag = "Untagged";

                        // Publish a radio tower event
                        EventBus.Publish<RadioTowerActivatedPlayerEvent>(new RadioTowerActivatedPlayerEvent(item.transform.parent.GetComponent<RadioTowerCode>().towerCode, item));
                    }
                }
                else if (item != null && (item.name is "Objective") && player.GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.RepairKit))
                {
                    RepairKitUse rk = item.GetComponent<RepairKitUse>();

                    if (rk != null)
                    {
                        //Debug.Log("Using Kit");
                        rk.UseKit();

                        EventBus.Publish(new RepairKitUsedEvent());

                        ActivePlayerInventory inventory = player.GetComponent<ActivePlayerInventory>();
                        inventory.useItem(ActivePlayerInventory.activePlayerItems.RepairKit);
                    }
                }

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
        //Debug.Log(gameObject.name);
        //Debug.Log(other.name);

        //Debug.Log($"tag = {other.gameObject.tag}, canInteractWithObject = {canInteractWithObject(other.gameObject)}");

        if ((other.gameObject.tag is "Pickup" || other.gameObject.tag is "Interactable" || other.gameObject.tag is "Objective") && !itemsInRange.Contains(other.gameObject) && canInteractWithObject(other.gameObject))
        {
            itemsInRange.Add(other.gameObject);
            //Debug.Log($"Adding item {other.gameObject.name} to range. count = {itemsInRange.Count}");
            EventBus.Publish(new newItemInPickupRangeEvent(itemsInRange.Count));
        }
    }

    // When you walk away from a pickupable item
    private void OnTriggerExit(Collider other)
    {
        if ((other.gameObject.tag is "Pickup" || other.gameObject.tag is "Interactable" || other.gameObject.tag is "Objective") && itemsInRange.Contains(other.gameObject))
        {
            itemsInRange.Remove(other.gameObject);
            //Debug.Log($"removing item to range. count = {itemsInRange.Count}");
            EventBus.Publish(new itemRemovedFromPickupRangeEvent(itemsInRange.Count));
        }
    }

    void OnInteract(InputValue value)
    {
        buttonPressed = value.isPressed;
    }

    private bool canInteractWithObject(GameObject item)
    {
        // If there is an item
        if (item != null && item.tag is "Pickup")
        {
            return true;
        }
        else if (item != null && item.tag is "Interactable")
        {
            //Debug.Log("Interactable silo, but cannot load rn");
            if ((item.transform.parent.name is "MissileSilo" || item.transform.parent.name is "MissileSilo(Clone)") && !item.transform.parent.GetComponent<MissileSiloStatus>().isSiloLoaded() && player.GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.NukeParts))
            {
                return true;
            }
            else if ((item.transform.parent.name is "RadioTower" || item.transform.parent.name is "RadioTower(Clone)") && !radioTowerInteract)
            {
                return true;
            }
            //if ( !((item.transform.parent.name is "MissileSilo" || item.transform.parent.name is "MissileSilo(Clone)") && !item.transform.parent.GetComponent<MissileSiloStatus>().isSiloLoaded() && !player.GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.NukeParts)))
            //{
            //    return true;
            //}
        }
        else if (item != null && (item.name is "Objective") && player.GetComponent<ActivePlayerInventory>().itemInInventory(ActivePlayerInventory.activePlayerItems.RepairKit))
        {
            return true;
        }

        return false;
    }

    // For when the player starts a tower
    public void playerStartedTower(RadioTowerActivatedPlayerEvent e)
    {
        radioTowerInteract = true;
    }

    // For when the manager completes the minigame
    public void managerEndedTower(RadioTowerActivatedManagerEvent e)
    {
        radioTowerInteract = false;
    }

    // For when the manager aborts the minigame
    public void managerEndedTowerAbort(miniGameAbortEvent e)
    {
        radioTowerInteract = false;
    }
}
