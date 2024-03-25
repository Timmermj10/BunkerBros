using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;

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
    public InventoryItem purchasedItem = new InventoryItem();

    public PurchaseEvent(InventoryItem item)
    {
        purchasedItem = item;
    }
}

// Class for picking up items
public class PickUpEvent
{
    public ActivePlayerInventory.activePlayerItems pickedUpItem;

    public PickUpEvent(ActivePlayerInventory.activePlayerItems item)
    {
        pickedUpItem = item;
    }
}   

// Class for the gold chest enemies
public class GoldChestEvent
{
    public bool entering;

    public GoldChestEvent(bool e)
    {
        entering = e;
    }
}

public class InteractTimerStartedEvent
{
    public float duration;

    public InteractTimerStartedEvent(float duration)
    {
        this.duration = duration;
    }

}

public class InteractTimerEndedEvent
{
}

public class SiloLoadedEvent
{
    public MissileSiloStatus status;

    public SiloLoadedEvent(MissileSiloStatus e)
    {
        status = e;
    }
}

public class WaveEndedEvent
{
}

public class WaveStartedEvent
{
}