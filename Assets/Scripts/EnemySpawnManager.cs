using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    public GameObject EnemyPrefab;
    public float spawnDelay = 200f;
    public float random_spawn = 10f;
    private float randomX;
    private float randomZ;
    private float initDelay;

    private int noSpawnZoneDistance = 3;

    WaveManager waveManager;

    private void Start()
    {
        // Set the initial delay
        initDelay = spawnDelay;

        waveManager = GameObject.FindAnyObjectByType<WaveManager>();

        EventBus.Subscribe<WaveStartedEvent>(WaveStarted);

    }

    private void WaveStarted(WaveStartedEvent e)
    {
        StartCoroutine(SpawnEnemiesForWave());
    }


    private IEnumerator SpawnEnemiesForWave()
    {

        while (waveManager.getNumEnemiesSpawnedSoFar() < waveManager.getNumEnemiesToSpawnThisRound())
        {
            do
            {
                randomX = Random.Range(-random_spawn, random_spawn);
            } while (Mathf.Abs(randomX) < noSpawnZoneDistance);

            do
            {
                randomZ = Random.Range(-random_spawn, random_spawn);
            } while (Mathf.Abs(randomZ) < noSpawnZoneDistance);


            //Make sure the maximum amount of enemies is not exceeded and the amount of enemies per wave is not exceeded
            if (spawnDelay <= 0 && waveManager.getNumEnemiesAlive() < waveManager.getMaxEnemiesAliveAtOnce())
            {
                // Instantiate the enemy
                GameObject enemy = Instantiate(EnemyPrefab);

                // Set the enemies position
                enemy.transform.position = new Vector3(transform.position.x + randomX, 1f, transform.position.z + randomZ);

                // Debug Statement
                //Debug.Log($"Spawning enemy at X: {enemy.transform.position.x}, Z: {enemy.transform.position.z}");

                // Reset the spawn timer
                spawnDelay = initDelay;

                //Let the waveManager know an enemy has been spawned
                waveManager.enemySpawned();
            }
            else
            {
                // Decrement spawn timer
                spawnDelay -= 1;
            }

            yield return new WaitForFixedUpdate();
        }

    }

}
