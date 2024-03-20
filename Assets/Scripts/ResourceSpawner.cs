using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject coin_prefab;
    private Vector3 location;
    private Subscription<ObjectDestroyedEvent> spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = EventBus.Subscribe<ObjectDestroyedEvent>(_spawn);
    }


    void _spawn(ObjectDestroyedEvent e)
    {

        //If an enemy died
        if (e.tag == "Enemy")
        {
            //get the location of the death
            location = e.deathCoordinates;

            //spawn a coin at that location
            Instantiate(coin_prefab, location, Quaternion.identity);
        }

    }
}
