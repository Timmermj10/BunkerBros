using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{

    private PingManager pingManager;
    private PopUpSystem popUpSystem;
    private GameObject bunker;

    public GameObject basicEnemyPrefab;
    public GameObject armoredEnemyPrefab;
    public GameObject anchoredEnemyPrefab;

    private int enemiesAlive = 0;
    private bool hasDroppedRepairKit = false;

    private bool hasPickedUpRepairKit = false;
    private bool hasUsedRepairKit = false;
    private bool hasPickedUpNukeParts = false;
    private bool hasPickedUpHealthPack = false;
    private bool hasFoundChest = false;
    private bool healthPackPopUpIsDone = false;
    private bool hasRespawnedPlayer = false;
    private bool hasActivatedRadioTower = false;

    private bool hasLoadedSilo = false;
    private bool hasBlownUpBoulder = false;

    public GameObject playerRespawn;
    public GameObject RepairKit;
    public GameObject AmmoCrate;
    public GameObject Gun;
    public GameObject Wall;
    public GameObject Turret;
    public GameObject HealthPack;
    public GameObject Nuke;
    public GameObject Missile;
    public GameObject NukeParts;
    public GameObject EvacuationButton;

    [SerializeField]
    private InputActionAsset actionAsset;
    private InputActionMap managerActionMap;
    private InputActionMap playerActionMap;

    private int activateNum = 0;


    void Start()
    {
        Debug.Log("Running tutorialManager start");
        EventBus.Subscribe<ObjectDestroyedEvent>(_enemyDeath);
        EventBus.Subscribe<AirdropLandedEvent>(_hasDroppedItems);
        EventBus.Subscribe<PickUpEvent>(_hasPickedUpItems);
        EventBus.Subscribe<SiloLoadedEvent>(_hasLoadedSilo);
        EventBus.Subscribe<RepairKitUsedEvent>(_hasUsedRepairKit);
        EventBus.Subscribe<PlayerRespawnEvent>(_playerRespawn);
        EventBus.Subscribe<CoinCollect>(_hasFoundChest);
        EventBus.Subscribe<PopUpEndEvent>(_endPopUp);
        EventBus.Subscribe<RadioTowerActivatedEvent>(_radioTowerActivated);
        EventBus.Subscribe<ItemUseEvent>(_ItemPurchased);


        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();
        popUpSystem = GameObject.Find("GameManager").GetComponent<PopUpSystem>();
        bunker = GameObject.Find("Objective");

        managerActionMap = actionAsset.FindActionMap("ManagerPlayer");
        playerActionMap = actionAsset.FindActionMap("ActivePlayer");

        playerRespawn.SetActive(false);
        RepairKit.SetActive(false);
        AmmoCrate.SetActive(false);
        Gun.SetActive(false);
        Wall.SetActive(false);
        Turret.SetActive(false);
        HealthPack.SetActive(false);
        Nuke.SetActive(false);
        Missile.SetActive(false);
        NukeParts.SetActive(false);
        EvacuationButton.SetActive(false);

        //SPAWN ANCHORED ENEMIES FOR RADIO TOWER

        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForFixedUpdate();

        bunker.GetComponent<HasHealth>().changeHealth(-10);
        bunker.GetComponent<HasHealth>().changeHealth(-50);

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "Your bunker is under attack! Don't let the zombies break in, your lives depend on it! Deploy your partner to handle the zombies on the surface.");
        playerRespawn.SetActive(true);

        while (!hasRespawnedPlayer)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Player");
        popUpSystem.popUp("Player", "Use the left and right joysticks to move/look and use RT to attack!");

        while (enemiesAlive > 0)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "Your bunker has taken damage! Drop in a repair kit to your partner to patch it up.");

        while (!hasDroppedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);

        startPopUp("Player");
        popUpSystem.popUp("Player", "Hold square (ps5) on the repair kit to pick it up.");

        while (!hasPickedUpRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Player");
        popUpSystem.popUp("Player", "Hold square (ps5) on the bunker to repair the hatch.");

        while (!hasUsedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "You're low on gold! Work together with your partner to find some! (Use WASD to move)");

        while (!hasFoundChest)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "You and your partner need a way to destroy that massive boulder blocking the way. Maybe these nuke parts could help.");

        while (!hasPickedUpNukeParts)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Player");
        popUpSystem.popUp("Player", "Load the nuke parts into the silo with square (ps5).");

        while (!hasLoadedSilo)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "You now have access to a nuke!");

        while (!hasBlownUpBoulder)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "Drop your partner in a healthpack, so they're prepared for their next fight!");

        while (!hasPickedUpHealthPack)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Player");
        popUpSystem.popUp("Player", "You now have a health pack! Press LB at any time to use it!");

        while (!healthPackPopUpIsDone)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);

        startPopUp("Player");
        popUpSystem.popUp("Player", "Go activate the radio tower to increase your signal strength! If you get your signal strength high enough, you can radio for an extraction team!");


        startPopUp("Manager");
        popUpSystem.popUp("Manager", "Theres a zombie horde approaching from the southwest! Use some walls, turrets and missiles to defend the bunker while your partner is activating the radio tower.");
        EventBus.Publish(new FirstTutorialWaveEvent());
        enemiesAlive = 7;

        while (enemiesAlive > 0)
        {
            yield return new WaitForFixedUpdate();
        }

        startPopUp("Manager");
        popUpSystem.popUp("Manager", "Great work! Here are some more supplies to help your partner. Drop them in a gun and ammo when you get enough gold to help them survive and thrive.");
        Gun.SetActive(true);
        AmmoCrate.SetActive(true);

        //turn on the evac button
        EvacuationButton.SetActive(true);

        //Make all buttons interactable again
        playerRespawn.GetComponent<Button>().interactable = true;
        RepairKit.GetComponent<Button>().interactable = true;
        AmmoCrate.GetComponent<Button>().interactable = true;
        Gun.GetComponent<Button>().interactable = true;
        Wall.GetComponent<Button>().interactable = true;
        Turret.GetComponent<Button>().interactable = true;
        HealthPack.GetComponent<Button>().interactable = true;
        Nuke.GetComponent<Button>().interactable = true;
        Missile.GetComponent<Button>().interactable = true;
        NukeParts.GetComponent<Button>().interactable = true;
        EvacuationButton.GetComponent<Button>().interactable = true;

        EventBus.Publish(new TutorialEndedEvent());

        yield return null;
    }

    private void startPopUp(string playerToFreeze)
    {
        EventBus.Publish(new PopUpStartEvent());
        
        if (playerToFreeze == "Manager") 
        {
            //Debug.Log("Disabling manager Player");
            managerActionMap.Disable();
        }
        else if (playerToFreeze == "Player")
        {
            //Debug.Log("Disabling active Player");
            playerActionMap.Disable();
        } 
        else
        {
            //Debug.Log("Disabling both players");
            managerActionMap.Disable();
            playerActionMap.Disable();
        }
    }

    private void _endPopUp(PopUpEndEvent e)
    {
        //Debug.Log("Turning Controls back on");
        managerActionMap.Enable();
        playerActionMap.Enable();
        activateNum++;

        switch (activateNum)
        {
            case 1:
                Instantiate(basicEnemyPrefab, new Vector3(-2, 1, 0), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(1, 1, 1.5f), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(1, 1, -1.5f), Quaternion.identity);
                enemiesAlive = 3;
                break;
            case 3:
                RepairKit.SetActive(true);
                break;
            case 7:
                NukeParts.SetActive(true);
                break;
            case 8:
                StartCoroutine(pingManager.Ping(new Vector3(10, 1, 1), 10, PingType.INVESTIGATE));
                break;
            case 9:
                StartCoroutine(pingManager.Ping(new Vector3(-11, 2, -2), 10));
                Nuke.SetActive(true);
                break;
            case 10:
                HealthPack.SetActive(true);
                break;
            case 11:
                healthPackPopUpIsDone = true;
                break;
            case 12:
                StartCoroutine(pingManager.Ping(new Vector3(-24, 5, -3), 10));
                break;
            case 13:
                StartCoroutine(pingManager.Ping(new Vector3(-5, 1, -22), 10, PingType.ENEMY));
                Instantiate(basicEnemyPrefab, new Vector3(-6, 1, -23), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(-6, 1, -21f), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(-4, 1, -23f), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(-4, 1, -21), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(-3, 1, -21), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(-3, 1, -23), Quaternion.identity);
                Instantiate(armoredEnemyPrefab, new Vector3(-5, 1, -22), Quaternion.identity);
                Wall.SetActive(true);
                Turret.SetActive(true);
                Missile.SetActive(true);
                break;
            case 14:
                EventBus.Publish(new WaveEndedEvent());
                break;
            default:
                break;
        }
    }

    private void _enemyDeath(ObjectDestroyedEvent e)
    {

        if (e.tag is "Enemy")
        {
            enemiesAlive--;
        }

        if (e.tag is "Boulder")
        {
            hasBlownUpBoulder = true;
        }
    }

    private void _hasDroppedItems(AirdropLandedEvent e)
    {
        if (e.itemID == 7)
        {
            hasDroppedRepairKit = true;
        }
    }

    private void _hasPickedUpItems(PickUpEvent e)
    {
        if (e.pickedUpItem == ActivePlayerInventory.activePlayerItems.RepairKit)
        {
            hasPickedUpRepairKit = true;
        }
        else if (e.pickedUpItem == ActivePlayerInventory.activePlayerItems.NukeParts)
        {
            hasPickedUpNukeParts = true;
        }
        else if (e.pickedUpItem == ActivePlayerInventory.activePlayerItems.HealthPack)
        {
            hasPickedUpHealthPack = true;
        }
    }

    private void _hasUsedRepairKit(RepairKitUsedEvent e)
    {
        hasUsedRepairKit = true;
    }

    private void _hasLoadedSilo(SiloLoadedEvent e)
    {
        hasLoadedSilo = true;
    }

    private void _hasFoundChest(CoinCollect e)
    {
        if (e.value > 100)
        {
            hasFoundChest = true;
        }
    }

    private void _playerRespawn(PlayerRespawnEvent e)
    {
        hasRespawnedPlayer = true;
    }

    private void _radioTowerActivated(RadioTowerActivatedEvent e)
    {
        if (!hasActivatedRadioTower)
        {
            startPopUp("Player");
            popUpSystem.popUp("Player", "Good job activating the radio tower! If you activate the rest and get your signal strength high enough you can radio for help!");
        }
    }

    private void _ItemPurchased(ItemUseEvent e)
    {
        switch (e.itemID)
        {
            case 0:
                NukeParts.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("Setting Selected Object to null");
                break;
            case 6:
                HealthPack.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("Setting Selected Object to null");
                break;
            case 7:
                RepairKit.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                Debug.Log("Setting Selected Object to null");
                break;
            default:
                break;
        }
    }

}
