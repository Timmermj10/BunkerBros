using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureSpawner : MonoBehaviour
{

    public GameObject wallPrefab;
    public GameObject nukePartsPrefab;

    void Start()
    {
        EventBus.Subscribe<AirdropLandedEvent>(_SpawnItems);
    }


    private void _SpawnItems(AirdropLandedEvent e)
    {
        if (e.itemID == 0)
        {
            Instantiate(nukePartsPrefab, e.itemLocation, Quaternion.identity);
        }
        else if (e.itemID == 1)
        {
            //Debug.Log("Spawned Wall");
            Instantiate(wallPrefab, e.itemLocation, Quaternion.identity);
        }
    }
}
