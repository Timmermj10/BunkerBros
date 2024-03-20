using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEvent
{
}
public class AirstrikeEvent
{
    public float x_cord;
    public float y_cord;
    public float z_cord;

    public AirstrikeEvent(Vector3 coordinates)
    {
        x_cord = coordinates.x;
        y_cord = coordinates.y;
        z_cord = coordinates.z;
    }
}

public class ItemUseEvent
{
    public int itemID;

    public ItemUseEvent(int itemUsed)
    {
        itemID = itemUsed;
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
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public int itemCount;
    public bool isOneTimePurchase;
    //public InventoryItem item;
    //TODO: Consolidate
    public PurchaseEvent(InventoryItem item)
    {
        itemID = item.itemId;
        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemCount = item.itemCount;
        isOneTimePurchase = item.oneTimePurchase;
    }
}