using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{

    //Enemy type prefabs
    public GameObject BasicEnemyPrefab;
    public GameObject ArmoredEnemyPrefab;


    Transform playerTransform;


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
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);

        playerTransform = GameObject.Find("player").transform;

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
                //if the player is currently dead
                if (playerTransform == null)
                {
                    Debug.Log("Player is dead, creating dummy transform");
                    playerTransform = transform;
                }

                // Reset the spawn timer
                spawnDelay = initDelay;

                //Debug.Log($"Comparing {randomVal} to {60 * ((float)waveManager.getNumEnemiesSpawnedSoFar() / waveManager.getNumEnemiesToSpawnThisRound())}");

                float progressFraction = (float)(waveManager.getNumEnemiesSpawnedSoFar() + 1) / waveManager.getNumEnemiesToSpawnThisRound();
                bool hasSpawnedEnoughEnemies = progressFraction > 0.2f;
                bool armoredSpawnChance = Random.value < 0.8f * progressFraction; //0.8 is scale factor to decrease spawn chance
                bool canSpawnMoreArmored = waveManager.getNumArmoredSpawnedSoFar() < waveManager.getNumArmoredToSpawnThisRound();

                if (hasSpawnedEnoughEnemies && armoredSpawnChance && canSpawnMoreArmored)
                {
                    // Instantiate the enemy
                    GameObject enemy = Instantiate(ArmoredEnemyPrefab);

                    // Set the enemies position
                    enemy.transform.position = new Vector3(playerTransform.position.x + randomX, 1f, playerTransform.position.z + randomZ);

                    //Let the waveManager know an enemy has been spawned
                    waveManager.enemySpawned(EnemyType.Armored);
                }
                else
                {
                    // Instantiate the enemy
                    GameObject enemy = Instantiate(BasicEnemyPrefab);

                    // Set the enemies position
                    enemy.transform.position = new Vector3(playerTransform.position.x + randomX, 1f, playerTransform.position.z + randomZ);

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

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        playerTransform = e.activePlayer.transform;
    }

}
