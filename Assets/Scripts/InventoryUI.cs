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

    private int inventoryItemsIndex = 0;

    public int equippedInventoryItemID = -1;

    public static InventoryItem equippedInventoryItem = null;

    // Subscribe to Purchase Events
    Subscription<PurchaseEvent> purchase_event_subscription;

    private GameObject ShopManager;

    // Inventory Icon / Text
    private Text inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        purchase_event_subscription = EventBus.Subscribe<PurchaseEvent>(_OnPurchase);
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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && inventoryItems.Count > 0)
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