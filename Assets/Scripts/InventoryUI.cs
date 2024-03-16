using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class InventoryUI : MonoBehaviour
{
    // Inventory starts empty, this will hold itemIDs of items that have been purchased
    public List<int> inventoryItems = new List<int> {};

    public int inventoryItemsIndex = 0;

    public int equippedInventoryItemID = -1;

    public static InventoryItem equippedInventoryItem = null;

    // Subscribe to Purchase Events
    Subscription<PurchaseEvent> purchase_event_subscription;

    // Subscribe to Item Use Events
    Subscription<ItemUseEvent> item_event_subscription;

    private GameObject ShopManager;

    // Inventory Icon / Text
    private Text inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        purchase_event_subscription = EventBus.Subscribe<PurchaseEvent>(_OnPurchase);
        item_event_subscription = EventBus.Subscribe<ItemUseEvent>(_OnUse);
        ShopManager = GameObject.Find("ShopManager");
        inventoryUI = GameObject.Find("Inventory").GetComponent<Text>();
    }

    void _OnPurchase(PurchaseEvent e)
    {
        // If the item is already purchased
        if (ShopManager.GetComponent<ShopManagerScript>().shopItems[e.itemID].itemCount == 1)
        {
            Debug.Log($"Purchased: {e.itemName} for the first time!");
            inventoryItems.Add(e.itemID);
        }
        Debug.Log($"Purchased: {e.itemName}" );

        // Update UI
        equippedInventoryItemID = inventoryItems[inventoryItemsIndex];
        equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
        inventoryUI.text = equippedInventoryItem.itemName;
        if (equippedInventoryItem.itemCount > 1)
        {
            inventoryUI.text += $" x {equippedInventoryItem.itemCount}";
        }
    }

    void _OnUse(ItemUseEvent e)
    {
        // Update the itemCount
        equippedInventoryItemID = e.itemID;
        equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
        equippedInventoryItem.itemCount = equippedInventoryItem.itemCount - 1;

        // Update UI
        if (equippedInventoryItem.itemCount > 1)
        {
            inventoryUI.text = $"{equippedInventoryItem.itemName} x {equippedInventoryItem.itemCount}";
        }
        else if ((equippedInventoryItem.itemCount == 1))
        {
            inventoryUI.text = equippedInventoryItem.itemName;
        }
        // If the item count is less than one, remove it from the inventory
        else
        {
            // Remove the item from the inventory
            inventoryItems.Remove(equippedInventoryItemID);

            // If there are no more items in the inventory
            if (inventoryItems.Count < 1)
            {
                equippedInventoryItemID = -1;
                inventoryUI.text = "Inventory Empty";
            }
            // If there is still an item in the inventory switch to showing that one
            else
            {
                inventoryItemsIndex = inventoryItemsIndex % inventoryItems.Count;
                equippedInventoryItemID = inventoryItems[inventoryItemsIndex];
                equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
                inventoryUI.text = equippedInventoryItem.itemName;
                if (equippedInventoryItem.itemCount > 1)
                {
                    inventoryUI.text += $" x {equippedInventoryItem.itemCount}";
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && inventoryItems.Count > 0)
        {
            inventoryItemsIndex = (inventoryItemsIndex + 1) % inventoryItems.Count;
            equippedInventoryItemID = inventoryItems[inventoryItemsIndex];
            equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
            inventoryUI.text = equippedInventoryItem.itemName;
            if (equippedInventoryItem.itemCount > 1) 
            {
                inventoryUI.text += $" x {equippedInventoryItem.itemCount}";
            }
        }
    }
}