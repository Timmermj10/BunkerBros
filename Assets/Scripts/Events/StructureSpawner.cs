using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{

    public GameObject wallPrefab;
    public GameObject nukePartsPrefab;
    public GameObject turretPrefab;

    void Start()
    {
        EventBus.Subscribe<AirdropLandedEvent>(_SpawnItems);
    }


    private void _SpawnItems(AirdropLandedEvent e)
    {
        if (e.itemID == 0)
        {
            //spawn nuke parts
            Instantiate(nukePartsPrefab, e.itemLocation, Quaternion.identity);
        }
        else if (e.itemID == 1)
        {
            //spawn wall with vertical offset for the wall being 2 blocks tall
            Instantiate(wallPrefab, e.itemLocation + new Vector3(0, 0.5f, 0), Quaternion.identity);
        }
        else if (e.itemID == 2)
        {
            //spawn turret
            Instantiate(turretPrefab, e.itemLocation + new Vector3(0, -0.4f, 0), Quaternion.identity);
        }
    }
}
