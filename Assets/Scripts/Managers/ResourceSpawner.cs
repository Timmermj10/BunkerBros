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
        if (e.tag == "Enemy" || e.tag == "GoldChestEnemy")
        {
            
            //get the location of the death and add a vertical offset of 1
            location = new Vector3(e.deathCoordinates.x, e.deathCoordinates.y + 1, e.deathCoordinates.z);

            //spawn a coin at that location
            GameObject coin = Instantiate(coin_prefab, location, Quaternion.identity);
            coin.GetComponent<Rigidbody>().AddForce(new Vector3(0, Random.value * 2, 0), ForceMode.Force);

            if (e.name == "EnemyArmoredNavMesh(Clone)")
            {
                for (int i = 0; i < 3; i++)
                {
                    coin = Instantiate(coin_prefab, location + new Vector3(Random.value, Random.value/2, Random.value), Quaternion.identity);
                    coin.GetComponent<Rigidbody>().AddForce(new Vector3(0, Random.value * 2, 0), ForceMode.Force);
                }
            }
        }

    }
}
