using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //Enemy type prefabs
    public GameObject BasicEnemyPrefab;
    public GameObject ArmoredEnemyPrefab;


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
                // Reset the spawn timer
                spawnDelay = initDelay;

                float randomVal = Random.Range(0f, 100f);

                //Debug.Log($"Comparing {randomVal} to {60 * ((float)waveManager.getNumEnemiesSpawnedSoFar() / waveManager.getNumEnemiesToSpawnThisRound())}");

                if ((((float)waveManager.getNumEnemiesSpawnedSoFar() + 1) / waveManager.getNumEnemiesToSpawnThisRound() > 0.2f) && randomVal < 80 * ((float)waveManager.getNumEnemiesSpawnedSoFar() / waveManager.getNumEnemiesToSpawnThisRound()) && waveManager.getNumArmoredSpawnedSoFar() < waveManager.getNumArmoredToSpawnThisRound())
                {
                    // Instantiate the enemy
                    GameObject enemy = Instantiate(ArmoredEnemyPrefab);

                    // Set the enemies position
                    enemy.transform.position = new Vector3(transform.position.x + randomX, 1f, transform.position.z + randomZ);

                    //Let the waveManager know an enemy has been spawned
                    waveManager.enemySpawned(EnemyType.Armored);
                }
                else
                {
                    // Instantiate the enemy
                    GameObject enemy = Instantiate(BasicEnemyPrefab);

                    // Set the enemies position
                    enemy.transform.position = new Vector3(transform.position.x + randomX, 1f, transform.position.z + randomZ);

                    //Let the waveManager know an enemy has been spawned
                    waveManager.enemySpawned(EnemyType.Basic);
                }
                //Debug.Log($"Enemies Spawned So far = {waveManager.getNumEnemiesSpawnedSoFar()} out of {waveManager.getNumEnemiesToSpawnThisRound()}");
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
