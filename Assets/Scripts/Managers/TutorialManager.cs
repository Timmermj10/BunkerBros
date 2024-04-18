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
    private GameObject bunker;

    //enemies
    public GameObject basicEnemyPrefab;
    public GameObject armoredEnemyPrefab;
    public GameObject anchoredEnemyPrefab;


    //timing variables
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
    private bool hasFinishedRadioTowerActivation = false;
    private bool hasBlownUpBoulder = false;
    private bool hasLoadedSilo = false;


    //reference to all the buttons
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
    
    //How long in between flashes for new buttons
    public float buttonFlashDuration = 0.3f;

    //int to keep track of timings for unlocking new player actions
    private int activateNum = 0;


    void Start()
    {
        //Debug.Log("Running tutorialManager start");
        EventBus.Subscribe<ObjectDestroyedEvent>(_enemyDeath);
        EventBus.Subscribe<AirdropLandedEvent>(_hasDroppedItems);
        EventBus.Subscribe<PickUpEvent>(_hasPickedUpItems);
        EventBus.Subscribe<SiloLoadedEvent>(_hasLoadedSilo);
        EventBus.Subscribe<RepairKitUsedEvent>(_hasUsedRepairKit);
        EventBus.Subscribe<PlayerRespawnEvent>(_playerRespawn);
        EventBus.Subscribe<CoinCollect>(_hasFoundChest);
        EventBus.Subscribe<PopUpEndEvent>(_endPopUp);
        EventBus.Subscribe<RadioTowerActivatedPlayerEvent>(_radioTowerActivated);
        EventBus.Subscribe<RadioTowerActivatedManagerEvent>(_RadioTowerActivatedByManager);
        EventBus.Subscribe<ItemUseEvent>(_ItemPurchased);


        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();
        bunker = GameObject.Find("Objective");

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


        EventBus.Publish(new PopUpStartEvent("Manager", "Your bunker is under attack! Don't let the zombies break in, your lives depend on it! Deploy your partner to handle the zombies on the surface.")); //1

        while (!hasRespawnedPlayer)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Player", "Use the left and right joysticks to move/look and use R2 to attack!")); //2

        while (enemiesAlive > 0)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "Your bunker has taken damage! Drop in a repair kit to your partner to patch it up.")); //3

        while (!hasDroppedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);

        EventBus.Publish(new PopUpStartEvent("Player", "Hold square on the repair kit to pick it up.")); //4

        while (!hasPickedUpRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Player", "Hold square on the bunker to repair the hatch.")); //5

        while (!hasUsedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "Drop your partner in a health pack, so they're prepared for their next fight!")); //6

        while (!hasPickedUpHealthPack)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Player", "You now have a health pack! Press L1 at any time to use it!")); //7

        while (!healthPackPopUpIsDone)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "You're low on gold! Work with your partner to find some! Use WASD to move and scroll to zoom. Use middle mouse button to ping for your partner!")); //8

        EventBus.Publish(new PopUpStartEvent("Player", "You're low on gold! Work with your partner to find some! Press your left stick in to sprint and X to jump!")); //9


        while (!hasFoundChest)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "You and your partner need a way to destroy that massive boulder blocking the way. Maybe these nuke parts could help.")); //10

        while (!hasPickedUpNukeParts)
        {
            yield return new WaitForFixedUpdate();
        }
        EventBus.Publish(new PopUpStartEvent("Player", "Load the nuke parts into the silo with square."));//11

        while (!hasLoadedSilo)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "You now have access to a nuke!"));//12

        while (!hasBlownUpBoulder)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1.5f);
        EventBus.Publish(new PopUpStartEvent("Player", "Go activate the radio tower to increase your signal strength! If you get your signal strength high enough, you can radio for an extraction team!")); //13

        while (!hasFinishedRadioTowerActivation)
        {
            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForSeconds(1f);

        EventBus.Publish(new PopUpStartEvent("Manager", "There's a zombie horde approaching from the north! Use some walls, turrets, and missiles to defend your bunker.", true, true));//15
        EventBus.Publish(new FirstTutorialWaveEvent());
        enemiesAlive = 7;

        while (enemiesAlive > 0)
        {
            yield return new WaitForFixedUpdate();
        }

        EventBus.Publish(new PopUpStartEvent("Manager", "Great work! There are some more supplies to help your partner. Help them out by dropping them a gun and some ammo if you get the chance.")); //16

        //turn on the evac button
        EvacuationButton.SetActive(true);

        //Make all buttons interactable again
        playerRespawn.GetComponent<Button>().interactable = true;
        RepairKit.GetComponent<Button>().interactable = true;
        //Dont enable Ammo crate until a gun is bought
        AmmoCrate.GetComponent<Button>().interactable = false;
        Gun.GetComponent<Button>().interactable = true;
        Wall.GetComponent<Button>().interactable = true;
        Turret.GetComponent<Button>().interactable = true;
        HealthPack.GetComponent<Button>().interactable = true;
        Missile.GetComponent<Button>().interactable = true;

        EventBus.Publish(new TutorialEndedEvent());
        EventBus.Publish(new WaveEndedEvent());

        yield return null;
    }

    private void _endPopUp(PopUpEndEvent e)
    {
        activateNum++;

        switch (activateNum)
        {
            case 1:
                bunker.GetComponent<HasHealth>().changeHealth(-10);
                Instantiate(basicEnemyPrefab, new Vector3(-2, 1, 0), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(1, 1, 1.5f), Quaternion.identity);
                Instantiate(basicEnemyPrefab, new Vector3(1, 1, -1.5f), Quaternion.identity);
                enemiesAlive = 3;

                playerRespawn.SetActive(true);
                StartCoroutine(ButtonFlashRoutine(playerRespawn));
                break;
            case 3:
                RepairKit.SetActive(true);
                StartCoroutine(ButtonFlashRoutine(RepairKit));
                break;
            case 5:
                //Ping Bunker so player knows what to repair
                pingManager.Ping(new Vector3(0, 1, 0), 10, PingType.INVESTIGATE);
                break;
            case 6:
                HealthPack.SetActive(true);
                StartCoroutine(ButtonFlashRoutine(HealthPack));
                break;
            case 7:
                healthPackPopUpIsDone = true;
                break;
            case 10:
                NukeParts.SetActive(true);
                StartCoroutine(ButtonFlashRoutine(NukeParts));
                pingManager.Ping(new Vector3(-3.9f, 0.94f, -20.3f), 10);
                break;
            case 11:
                //Ping Silo
                pingManager.Ping(new Vector3(1, 1.45f, 9.1f), 10, PingType.INVESTIGATE);
                break;
            case 12:
                //Ping Boulder
                pingManager.Ping(new Vector3(-3.9f, 0.94f, -20.3f), 10);
                Nuke.SetActive(true);
                StartCoroutine(ButtonFlashRoutine(Nuke));
                break;
            case 13:
                //Ping Radio Tower
                pingManager.Ping(new Vector3(-1.7f, 1.5f, -33.63f), 10);
                break;
            case 15:
                StartCoroutine(spawnTutorialWave());
                
                //Show the buttons
                Wall.SetActive(true);
                Turret.SetActive(true);
                Missile.SetActive(true);

                //Flash the buttons
                StartCoroutine(ButtonFlashRoutine(Wall));
                StartCoroutine(ButtonFlashRoutine(Turret));
                StartCoroutine(ButtonFlashRoutine(Missile));
                break;
            case 16:
                //Show the buttons
                Gun.SetActive(true);
                AmmoCrate.SetActive(true);

                //Flash the buttons
                StartCoroutine(ButtonFlashRoutine(Gun));
                StartCoroutine(ButtonFlashRoutine(AmmoCrate));
                break;
            default:
                break;
        }
    }

    private IEnumerator spawnTutorialWave()
    {
        //Ping Zombie spawn location
        pingManager.Ping(new Vector3(-1, 1.5f, 35), 10, PingType.ENEMY);

        yield return new WaitForSeconds(5f);
        Instantiate(basicEnemyPrefab, new Vector3(0, 0.15f, 33), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(-2, 0.15f, 33), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(0, 0.15f, 34), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(-2, 0.15f, 36), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(-2, 0.15f, 34), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(0, 0.15f, 36), Quaternion.identity);
        Instantiate(armoredEnemyPrefab, new Vector3(-1, 0.15f, 35), Quaternion.identity);

        yield return null;
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

    private void _RadioTowerActivatedByManager(RadioTowerActivatedManagerEvent e)
    {
        hasFinishedRadioTowerActivation = true;
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

    private void _radioTowerActivated(RadioTowerActivatedPlayerEvent e)
    {
        if (!hasActivatedRadioTower)
        {
            EventBus.Publish(new PopUpStartEvent("Player", "Good job activating the radio tower! Make sure you find the code to help get the tower fully online!")); //14
            hasActivatedRadioTower = true;
        }
    }

    private void _ItemPurchased(ItemUseEvent e)
    {
        switch (e.itemID)
        {
            case 0:
                NukeParts.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                //Debug.Log("Setting Selected Object to null");
                break;
            case 6:
                HealthPack.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                //Debug.Log("Setting Selected Object to null");
                break;
            case 7:
                RepairKit.GetComponent<Button>().interactable = false;
                EventSystem.current.SetSelectedGameObject(null);
                //Debug.Log("Setting Selected Object to null");
                break;
            default:
                break;
        }
    }


    private IEnumerator ButtonFlashRoutine(GameObject buttonGameObject)
    {
        //Debug.Log($"Flashing gameobject {buttonGameObject.name}");

        Button buttonToFlash = buttonGameObject.GetComponent<Button>();

        if (buttonToFlash != null)
        {
            Color originalColor = Color.white;

            for (int i = 0; i < 5; i++)
            {
                // Change the button color to green
                buttonToFlash.image.color = Color.green;

                // Wait for flashDuration to end
                yield return new WaitForSeconds(buttonFlashDuration);

                // Change the button color back to original color
                buttonToFlash.image.color = originalColor;

                // Wait for flashDuration to end
                yield return new WaitForSeconds(buttonFlashDuration);
            }
        }
    }

}
