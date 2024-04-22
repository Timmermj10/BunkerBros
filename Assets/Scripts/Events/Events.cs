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
    public Vector3 position;

    public SiloLoadedEvent(MissileSiloStatus e, Vector3 position)
    {
        status = e;
        this.position = position;
    }
}

// Event for when the silo is unloaded
public class SiloUnloadedEvent
{
    public MissileSiloStatus status;
    public Vector3 position;

    public SiloUnloadedEvent(MissileSiloStatus e, Vector3 position)
    {
        status = e;
        this.position = position;
    }
}

public class WaveEndedEvent
{
}

public class WaveStartedEvent
{
}

public class VictoryMusicEvent
{
}

public class DeathMusicEvent
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

public class PopUpStartEvent
{
    public string player;
    public string text;
    public bool freezeEnemies;
    public bool freezeBothPlayers;
    public PopUpStartEvent(string player, string text, bool freezeEnemies = true, bool freezeBothPlayers = false)
    {
        this.player = player;
        this.text = text;
        this.freezeEnemies = freezeEnemies;
        this.freezeBothPlayers = freezeBothPlayers;
    }
}

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
    public List<int> code;
    public GameObject transformerInteractable;

    public RadioTowerActivatedPlayerEvent(List<int> code, GameObject transformerInteractable)
    {
        this.code = code;
        this.transformerInteractable = transformerInteractable;
    }
}

public class RadioTowerActivatedManagerEvent
{
}

// Event for when the manager aborts the minigame
public class miniGameAbortEvent
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

public class LastWaveOverEvent
{
}

public class GameOverEvent
{
}

//Event to trigger swap into knife and sound
public class EmptyAmmo
{ }
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

public class zombieDamagedEvent
{
    public Vector3 position;

    public zombieDamagedEvent(Vector3 position)
    {
        this.position = position;
    }
}

public class zombieAttackEvent
{
    public Vector3 position;

    public zombieAttackEvent(Vector3 position)
    {
        this.position = position;
    }
}

public class KnifeAttackSoundEvent
{ } 

// Empty class for when the objective is being damaged
public class ObjectiveDamagedEvent
{
}

public class GameplayStartEvent
{
}

public class TurretShootingEvent
{
    public Vector3 position;

    public TurretShootingEvent(Vector3 position)
    {
        this.position = position;
    }
}

public class PlayerMovingEvent
{
    public Vector2 movementValue;
    public bool isSprinting;

    public PlayerMovingEvent(Vector2 movementValue, bool isSprinting)
    {
        this.movementValue = movementValue;
        this.isSprinting = isSprinting;
    }
}

public class WeaponSwapEvent
{
    public bool trueIsKnife;

    public WeaponSwapEvent(bool trueIsKnife)
    {
        this.trueIsKnife = trueIsKnife;
    }
}

public class ManagerButtonPress
{
}

public class ManagerIncorrectAnswer
{
}
