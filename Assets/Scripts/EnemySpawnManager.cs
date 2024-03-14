using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public float spawnDelay = 200f;
    public float random_spawn = 5f;
    private float random;
    private float initDelay;

    private void Start()
    {
        initDelay = spawnDelay;
    }

    void FixedUpdate()
    {
        random = Random.Range(-random_spawn, random_spawn);
        if (spawnDelay <= 0)
        {
            Instantiate(EnemyPrefab);
            Vector3 loca = transform.position;
            loca.x += random;
            EnemyPrefab.transform.position = loca;
            spawnDelay = initDelay;
        } else
        {
            spawnDelay -= 1;
        }
    }
}
