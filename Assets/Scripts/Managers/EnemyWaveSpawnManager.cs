using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawnManager : MonoBehaviour
{
    //Enemy type prefabs
    public GameObject BasicEnemyPrefab;
    public GameObject ArmoredEnemyPrefab;

    //List of spawnpoints

    private List<Vector3> spawnpoints = new List<Vector3>();

    //Time between enemy spawned
    private float spawnDelay = 2f;
    private float spawnTimer = 2f;

    //Enemy spawn offsets
    private float randomX;
    private float randomZ;

    //how far away enemies can spawn from the spawner (half the width/height of the map)
    private float spawnDistance = 4f;
    private int spawnIndex = 0;

    //Wave manager reference to get wave information
    WaveManager waveManager;


    void Start()
    {
        // Set the initial delay
        spawnTimer = spawnDelay;

        waveManager = FindAnyObjectByType<WaveManager>();

        EventBus.Subscribe<WaveStartedEvent>(_WaveStarted);

        spawnpoints.Add(new Vector3(-3, 1, -21)); //Bottom Left;
        spawnpoints.Add(new Vector3(5, 1, -21)); //Bottom Right
        spawnpoints.Add(new Vector3(-2, 1, 32)); //Top Left
        spawnpoints.Add(new Vector3(21, 1, 21)); //Top Right
        //Debug.Log($"spawnpoints length = {spawnpoints.Count}");

    }

    private void _WaveStarted(WaveStartedEvent e)
    {
        spawnDelay -= 0.1f;

        spawnIndex = Random.Range(0, spawnpoints.Count);

        Vector3 spawnpointForWave = spawnpoints[spawnIndex];

        //Debug.Log($"spawnpointForWave of {spawnpointForWave} chosen, because of index {spawnIndex}");

        StartCoroutine(SpawnEnemiesForWave(spawnpointForWave));
    }

    IEnumerator SpawnEnemiesForWave(Vector3 spawnpointForWave)
    {
        //Debug.Log("Spawning Enemies For Wave");
        while (waveManager.getNumEnemiesSpawnedSoFar() < waveManager.getNumEnemiesToSpawnThisRound())
        {

            //Make sure the maximum amount of enemies is not exceeded and the amount of enemies per wave is not exceeded
            if (spawnTimer <= 0 && waveManager.getNumEnemiesAlive() < waveManager.getMaxEnemiesAliveAtOnce())
            {
                // Reset the spawn timer
                spawnTimer = Random.Range(spawnDelay-1, spawnDelay+1);
                //Debug.Log($"Spawn timer reset to {spawnTimer}");

                
                Vector3 randomSpawnPosition;
                RaycastHit hitInfo;

                //Get a random position to spawn the enemy
                do
                {
                    // Decide whether to spawn the enemy on the horizontal (X-axis) or vertical (Z-axis) edges
                    if (Random.value < 0.5f)
                    {
                        // Spawn on the horizontal edges
                        randomX = Random.Range(-spawnDistance, spawnDistance); // Anywhere along the horizontal edge

                        randomZ = Random.Range(0, spawnDistance);

                        // Top or Bottom edge
                        if (Random.value < 0.5) randomZ *= -1;
                    }
                    else
                    {
                        // Spawn on the vertical edges
                        randomZ = Random.Range(-spawnDistance, spawnDistance); // Anywhere along the vertical edge

                        randomX = Random.Range(0, spawnDistance);

                        // Top or Bottom edge
                        if (Random.value < 0.5) randomX *= -1;
                    }

                    randomSpawnPosition = new Vector3(spawnpointForWave.x + randomX, 10f, spawnpointForWave.z + randomZ);
                    //Debug.Log($"Checking to see of the chosen spawn position ({randomSpawnPosition}) is valid");

                } while (!Physics.Raycast(randomSpawnPosition, Vector3.down, out hitInfo, Mathf.Infinity) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));

                randomSpawnPosition.y = hitInfo.point.y + 0.5f;
                //Debug.Log($"Final spawn position of {randomSpawnPosition} chosen | Raycast collided with {hitInfo.collider.gameObject.name}");

                //Decide on the type of enemy to spawn
                float progressFraction = (float)(waveManager.getNumEnemiesSpawnedSoFar() + 1) / waveManager.getNumEnemiesToSpawnThisRound();
                bool hasSpawnedEnoughEnemies = progressFraction > 0.2f;
                bool armoredSpawnChance = Random.value < (0.8f * progressFraction); //0.8 is scaling factor to decrease spawn chance for armoredEnemies
                bool canSpawnMoreArmored = waveManager.getNumArmoredSpawnedSoFar() < waveManager.getNumArmoredToSpawnThisRound();
                //Debug.Log($"progress fraction = {progressFraction}, hasSpawnedEnoughEnemies = {hasSpawnedEnoughEnemies}, armoredSpawnChance = {armoredSpawnChance}, canSpawnMoreArmored = {canSpawnMoreArmored}");

                if (hasSpawnedEnoughEnemies && armoredSpawnChance && canSpawnMoreArmored)
                {

                    //Chance to spawn a horde
                    if (Random.value <= 0.4f && waveManager.getNumHordesSpawnedSoFar() < waveManager.getNumHordesToSpawnThisRound())
                    {
                        //Spawn a horde with a minimum of 4 enemies and a maximum of maxEnemies/6
                        //Spawn radius is maximum of 3 and waveNumber/2
                        //Horde centered on randomSpawnPosition
                        //Debug.Log($"Spawning Horde at {randomSpawnPosition}");
                        spawnHorde(Mathf.Max(6, Mathf.RoundToInt(waveManager.getNumEnemiesToSpawnThisRound() / 6f)), Mathf.Max(3f, waveManager.getWaveNumber() / 2f), randomSpawnPosition);
                    }
                    else
                    {
                        //spawn an armored enemy
                        //Debug.Log("Spawning Armored");
                        spawnEnemy(EnemyType.Armored, randomSpawnPosition);
                    }
                }
                else
                {
                    //Debug.Log("Spawning Basic");
                    spawnEnemy(EnemyType.Basic, randomSpawnPosition);
                }
            }
            else
            {
                // Decrement spawn timer
                spawnTimer -= Time.deltaTime;
            }

            yield return new WaitForFixedUpdate();
        }
    }


    private void spawnEnemy(EnemyType type, Vector3 position)
    {
        //make sure were not exceeding number of spawns for the wave or maximum enemies alive at once
        if (waveManager.getNumEnemiesSpawnedSoFar() < waveManager.getNumEnemiesToSpawnThisRound() && waveManager.getNumEnemiesAlive() < waveManager.getMaxEnemiesAliveAtOnce())
        {
            GameObject enemy = null;
            //Spawn the correct enemy type at the specified position
            if (type is EnemyType.Basic)
            {
                enemy = Instantiate(BasicEnemyPrefab, position, Quaternion.identity);
            }
            else if (type is EnemyType.Armored)
            {
                enemy = Instantiate(ArmoredEnemyPrefab, position, Quaternion.identity);
            }
            //let the wave manager know an enemy spawned
            waveManager.enemySpawned(type);
            enemy.GetComponent<HasHealth>().setHealth(enemy.GetComponent<HasHealth>().getMaxHealth() + waveManager.getWaveNumber() - 1);
        }
    }

    private void spawnHorde(int hordeSize, float spawnRadius, Vector3 centerPoint)
    {

        //Calculate the number of armoredEnemies to spawn in the horde, scaling for wave number while not exceededing max armored spawns
        int maxArmoredToSpawnInHorde = Mathf.Min(waveManager.getNumArmoredToSpawnThisRound() - waveManager.getNumArmoredSpawnedSoFar(), Mathf.Max(1, Mathf.RoundToInt(waveManager.getWaveNumber() / 3f)));
        int armoredSpawnedInHorde = 0;


        //25% chance to have a horde without Armored Enemies
        if (Random.value < 0.25)
        {
            maxArmoredToSpawnInHorde = 0;
        }

        Vector3 spawnPos;

        for (int i = 0; i < hordeSize; i++)
        {
            RaycastHit hitInfo;
            do
            {
                // Generate a random position within a sphere
                spawnPos = centerPoint + Random.insideUnitSphere * spawnRadius;
                spawnPos.y = 10f;
            }
            while (!Physics.Raycast(spawnPos, Vector3.down, out hitInfo, Mathf.Infinity) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));

            //reset y position
            spawnPos.y = hitInfo.point.y + 0.5f;

            //Choose between enemy types
            if (armoredSpawnedInHorde < maxArmoredToSpawnInHorde)
            {
                armoredSpawnedInHorde++;
                spawnEnemy(EnemyType.Armored, spawnPos);
            }
            else
            {
                spawnEnemy(EnemyType.Basic, spawnPos);
            }
        }
    }

}
