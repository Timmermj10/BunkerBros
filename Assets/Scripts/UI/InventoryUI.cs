using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

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

    //Subscribe to Inventory Cycle Events
    Subscription<ManagerCycleEvent> manager_event_subscription;

    //Subscribe to Silo Loaded Events
    Subscription<SiloLoadedEvent> silo_loaded_event_subscription;

    private GameObject ShopManager;

    // Inventory Icon / Text
    private Text inventoryUI;

    // Start is called before the first frame update
    void Start()
    {
        purchase_event_subscription = EventBus.Subscribe<PurchaseEvent>(_OnPurchase);
        item_event_subscription = EventBus.Subscribe<ItemUseEvent>(_OnUse);
        manager_event_subscription = EventBus.Subscribe<ManagerCycleEvent>(_OnCycle);
        silo_loaded_event_subscription = EventBus.Subscribe<SiloLoadedEvent>(_SiloLoadedInventory);
        ShopManager = GameObject.Find("ShopManager");
        inventoryUI = GameObject.Find("Inventory").GetComponent<Text>();
    }

    void _OnPurchase(PurchaseEvent e)
    {
        // If the item has not been purchased before and it's not one time purchasable
        if (ShopManager.GetComponent<ShopManagerScript>().shopItems[e.purchasedItem.itemId].itemCount == 1 && !e.purchasedItem.oneTimePurchase)
        {
            //Debug.Log($"Purchased: {e.itemName} for the first time!");
            inventoryItems.Add(e.purchasedItem.itemId);
        }
        //Debug.Log($"Purchased: {e.itemName}" );

        // If the purchase is a one time purchase
        if (e.purchasedItem.oneTimePurchase)
        {
            StartCoroutine(destroyShopItem(e));
        }

        // Only update manager inventory UI if it's multiple purchases

        // ONLY FOR NOW, MAYBE AIRDROPS LATER

        else
        {
            // Update UI
            equippedInventoryItemID = inventoryItems[inventoryItemsIndex];
            equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
            inventoryUI.text = equippedInventoryItem.itemName;
            if (equippedInventoryItem.itemCount > 1)
            {
                inventoryUI.text += $" x {equippedInventoryItem.itemCount}";
            }
        }
    }

    IEnumerator destroyShopItem(PurchaseEvent e)
    {
        // Wait 0.25 second before destroying the button for the one time purchase
        yield return new WaitForSeconds(0.25f);

        // Get the item game object
        GameObject item = GameObject.Find($"Item{e.purchasedItem.itemId + 1}");

        // Destroy game object for the one time purchase item
        Destroy(item);

        //Debug.Log($"One time purchase of {item}");

        yield return null;
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

    /*
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
    */

    private void _OnCycle(ManagerCycleEvent e)
    {
        if (inventoryItems.Count > 0)
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

    void _SiloLoadedInventory(SiloLoadedEvent e) 
    {
        // If the item has not been purchased before and it's not one time purchasable
        if (ShopManager.GetComponent<ShopManagerScript>().shopItems[4].itemCount == 0)
        {
            inventoryItems.Add(4);
        }

        // Increment count in shop
        ShopManager.GetComponent<ShopManagerScript>().shopItems[4].itemCount += 1;

        // Only update manager inventory UI if it's multiple purchases

        // ONLY FOR NOW, MAYBE AIRDROPS LATER

        // Update UI
        equippedInventoryItemID = inventoryItems[inventoryItemsIndex];
        equippedInventoryItem = ShopManager.GetComponent<ShopManagerScript>().shopItems[equippedInventoryItemID];
        inventoryUI.text = equippedInventoryItem.itemName;
        if (equippedInventoryItem.itemCount > 1)
        {
            inventoryUI.text += $" x {equippedInventoryItem.itemCount}";
        }


    }
}