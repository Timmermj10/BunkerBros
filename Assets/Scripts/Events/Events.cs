using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

public class AirDropStartedEvent
{
    public int itemID;
    public Transform airdropTransform;
    public AirDropStartedEvent(int itemUsed, Transform airdropTransform)
    {
        this.itemID = itemUsed;
        this.airdropTransform = airdropTransform;
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

// Event for when the silo is loaded
public class SiloLoadedEvent
{
    public MissileSiloStatus status;

    public SiloLoadedEvent(MissileSiloStatus e)
    {
        status = e;
    }
}

// Event for when the silo is unloaded
public class SiloUnloadedEvent
{
    public MissileSiloStatus status;

    public SiloUnloadedEvent(MissileSiloStatus e)
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

public class ShootEvent
{
}
// Event for when buttons in the Manager UI are clicked
public class ManagerButtonClickEvent
{
    public Button button;

    public ManagerButtonClickEvent(Button b)
    {
        button = b;
    }
}

public class PlayerDamagedEvent
{
    public int health;

    public PlayerDamagedEvent(int playerHealth)
    {
        health = playerHealth;
    }
}

public class RepairKitUsedEvent { }

public class PopUpStartEvent{ }

public class PopUpEndEvent 
{
    public string player;

    public PopUpEndEvent(string player)
    {
        this.player = player;
    }
}

// Class for the spark and blood animations
public class DamageEffectEvent
{
    public GameObject zombie;
    public bool blood;

    public DamageEffectEvent(GameObject zombie, bool blood)
    {
        this.zombie = zombie;
        this.blood = blood;
    }
}

// Class for turning on the radio towers
public class RadioTowerActivatedPlayerEvent
{
}

public class RadioTowerActivatedManagerEvent
{
}

public class TutorialEndedEvent
{
}

//Event to indicate when the tutorial wave starts
public class FirstTutorialWaveEvent
{
}

// Event to show that the next wave should be the last
public class LastWaveEvent
{
}

public class newItemInPickupRangeEvent
{
    public int numItemsInRange;

    public newItemInPickupRangeEvent(int numItemsInRange)
    {
        this.numItemsInRange = numItemsInRange;
    }
}

public class itemRemovedFromPickupRangeEvent
{
    public int numItemsInRange;

    public itemRemovedFromPickupRangeEvent(int numItemsInRange)
    {
        this.numItemsInRange = numItemsInRange;
    }
}