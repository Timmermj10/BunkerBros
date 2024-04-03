using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TutorialManager : MonoBehaviour
{

    private PopUpSystem popUpSystem;
    private GameObject bunker;

    public GameObject basicEnemyPrefab;

    private int enemiesAlive = 0;
    private bool hasDroppedRepairKit = false;

    private bool hasPickedUpRepairKit = false;
    private bool hasUsedRepairKit = false;
    private bool hasPickedUpNukeParts = false;
    private bool hasPickedUpHealthPack = false;
    private bool hasPickedUpAmmo = false;
    private bool hasFoundChest = false;

    private bool hasRespawnedPlayer = false;

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


    void Start()
    {

        EventBus.Subscribe<ObjectDestroyedEvent>(_enemyDeath);
        EventBus.Subscribe<AirdropLandedEvent>(_hasDroppedItems);
        EventBus.Subscribe<PickUpEvent>(_hasPickedUpItems);
        EventBus.Subscribe<SiloLoadedEvent>(_hasLoadedSilo);
        EventBus.Subscribe<RepairKitUsedEvent>(_hasUsedRepairKit);
        EventBus.Subscribe<PlayerRespawnEvent>(_playerRespawn);
        EventBus.Subscribe<CoinCollect>(_hasFoundChest);

        popUpSystem = GameObject.Find("TutorialPopUpSystem").GetComponent<PopUpSystem>();
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

        StartCoroutine(Tutorial());
    }

    IEnumerator Tutorial()
    {
        yield return new WaitForFixedUpdate();

        bunker.GetComponent<HasHealth>().changeHealth(-10);
        bunker.GetComponent<HasHealth>().changeHealth(-50);

        Instantiate(basicEnemyPrefab, new Vector3(-2, 1, 0), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(1, 1, 1.5f), Quaternion.identity);
        Instantiate(basicEnemyPrefab, new Vector3(1, 1, -1.5f), Quaternion.identity);
        enemiesAlive = 3;

        popUpSystem.popUp("Manager", "Your bunker is under attack! Don't let the zombies break in, your lives depend on it! Deploy your partner to handle the zombies on the surface.");

        playerRespawn.SetActive(true);

        while (!hasRespawnedPlayer)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Player", "Use the left and right joysticks to move/look and use RB to attack!");

        while (enemiesAlive > 0)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Manager", "Your bunker has taken damage! Drop in a repair kit to your partner to patch it up.");

        RepairKit.SetActive(true);

        while (!hasDroppedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }
        yield return new WaitForSeconds(1);

        popUpSystem.popUp("Player", "Hold square (ps5) on the repair kit to pick it up.");

        while (!hasPickedUpRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Player", "Hold square (ps5) on the bunker to repair the hatch.");

        while (!hasUsedRepairKit)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Manager", "You're low on money! Work together with your partner to find some!");

        while (!hasFoundChest)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Manager", "You and your partner need a way to get past that massive boulder blocking the way. Maybe these nuke parts could help.");

        NukeParts.SetActive(true);

        while (!hasPickedUpNukeParts)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Player", "Load the nuke parts into the silo with square (ps5).");

        while (!hasLoadedSilo)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Manager", "You now have access to a nuke!");

        Nuke.SetActive(true);

        while (!hasBlownUpBoulder)
        {
            yield return new WaitForFixedUpdate();
        }

        popUpSystem.popUp("Manager", "Great work! Use some walls, turrets and missiles to defend the bunker while your partner activates the radio tower.");

        Wall.SetActive(true);
        Turret.SetActive(true);
        Missile.SetActive(true);






        yield return null;
    }

    private void _enemyDeath(ObjectDestroyedEvent e)
    {

        if (e.tag is "Enemy")
        {
            enemiesAlive--;
        }

        if (e.tag is "boulder")
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

}
