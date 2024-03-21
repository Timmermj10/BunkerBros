using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent
{
}

public class ItemUseEvent
{
    public int itemID;
    public Vector3 itemLocation;
    public bool isAirdrop;

    public ItemUseEvent(int itemUsed, Vector3 itemLocation, bool isAirdrop)
    {
        this.itemID = itemUsed;
        this.itemLocation = itemLocation;
        this.isAirdrop = isAirdrop;
    }
}

public class AirdropLandedEvent
{
    public int itemID;
    public Vector3 itemLocation;

    public AirdropLandedEvent(int itemUsed, Vector3 itemLocation)
    {
        this.itemID = itemUsed;
        this.itemLocation = itemLocation;
    }
}

public class ManagerCycleEvent
{
}

public class ObjectDestroyedEvent
{
    public string name;
    public string tag;
    public Vector3 deathCoordinates;

    public ObjectDestroyedEvent(string name, string tag, Vector3 deathCoordinates)
    {
        this.name = name;
        this.tag = tag;
        this.deathCoordinates = deathCoordinates;
    }

}

public class PlayerRespawnEvent
{
    public Vector3 spawnCoordinates;
    public GameObject activePlayer;

    public PlayerRespawnEvent(Vector3 spawnCoordinates, GameObject activePlayer)
    {
        this.spawnCoordinates = spawnCoordinates;
        this.activePlayer = activePlayer;
    }
}

public class CoinCollect
{
    public int value;

    public CoinCollect(int _new_value) { value = _new_value; }
}

// Class for general item purchases
public class PurchaseEvent
{
    //public int itemID;
    //public string itemName;
    //public Sprite itemIcon;
    //public int itemCount;
    //public bool isOneTimePurchase;
    public InventoryItem purchasedItem = new InventoryItem();

    public PurchaseEvent(InventoryItem item)
    {
        purchasedItem.itemId = item.itemId;
        purchasedItem.itemName = item.itemName;
        purchasedItem.itemIcon = item.itemIcon;
        purchasedItem.itemCount = item.itemCount;
        purchasedItem.oneTimePurchase = item.oneTimePurchase;
    }
}

// Class for picking up items
public class PickUpEvent
{
    public InventoryItem pickedUpItem;
    public int itemID;
    public string itemName;

    public PickUpEvent(InventoryItem item)
    {
        pickedUpItem.itemId = item.itemId;
        pickedUpItem.itemName = item.itemName;
    }
}   