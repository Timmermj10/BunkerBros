using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public Direction direct = Direction.X;
    public float spawnDelay = 200f;
    public float random_spawn = 5f;
    private float randomX;
    private float randomZ;
    private float initDelay;

    Vector3 spawnerLocation;

    private void Start()
    {
        // Set the initial delay
        initDelay = spawnDelay;

        // Get the spawner location
        spawnerLocation = transform.position;
    }

    void FixedUpdate()
    {
        randomX = Random.Range(-random_spawn, random_spawn);
        randomZ = Random.Range(-random_spawn, random_spawn);

        if (spawnDelay <= 0)
        {
            // Instantiate the enemy
            GameObject enemy = Instantiate(EnemyPrefab);

            // Set the enemies position
            enemy.transform.position = new Vector3(spawnerLocation.x + randomX, 1f, spawnerLocation.z + randomZ);

            // Debug Statement
            Debug.Log($"Spawning enemy at X: {enemy.transform.position.x}, Z: {enemy.transform.position.z}");

            // Reset the spawn timer
            spawnDelay = initDelay;
        } else
        {
            // Decrement spawn timer
            spawnDelay -= 1;
        }
    }
}

public enum Direction
{
    X, Z
}
