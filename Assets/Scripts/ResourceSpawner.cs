using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    public GameObject coin_prefab;
    private Vector3 location;
    private Subscription<EnemyDefeat> spawner;
    // Start is called before the first frame update
    void Start()
    {
        spawner = EventBus.Subscribe<EnemyDefeat>(_spawn);
    }


    void _spawn(EnemyDefeat e)
    {
        
        location = e.spawn_location;
        Debug.Log("Spawning at " + location);
        coin_prefab.transform.position = location;
        Instantiate(coin_prefab);

    }
}
