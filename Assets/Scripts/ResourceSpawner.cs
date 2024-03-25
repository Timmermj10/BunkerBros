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
            //get the location of the death and add a vertical offset of 0.5
            location = new Vector3(e.deathCoordinates.x, 1f, e.deathCoordinates.z);

            //spawn a coin at that location
            Instantiate(coin_prefab, location, Quaternion.identity);
        }

    }
}
