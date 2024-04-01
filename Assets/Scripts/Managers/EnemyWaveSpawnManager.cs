using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawnManager : MonoBehaviour
{
    //Enemy type prefabs
    public GameObject BasicEnemyPrefab;
    public GameObject ArmoredEnemyPrefab;

    //Transform of the player so we can spawn enemies arounf the player
    Transform playerTransform;

    //Time between enemy spawned
    private float spawnDelay = 3f;
    private float spawnTimer = 3f;

    //Enemy spawn offsets
    private float randomX;
    private float randomZ;

    //how far away enemies can spawn from the spawner
    private float spawnDistanceForMap = 16f;
    private float spawnDistanceForPlayer = 6f;

    //cant spawn within this distance of the spawner (spawns the zombies on the edge of the map if the spawner is in the middle)
    private int noSpawnZoneDistanceForMap = 12;
    private int noSpawnZoneDistanceForPlayer = 3;

    //Wave manager reference to get wave information
    WaveManager waveManager;


    void Start()
    {
        // Set the initial delay
        spawnTimer = spawnDelay;

        waveManager = FindAnyObjectByType<WaveManager>();

        EventBus.Subscribe<WaveStartedEvent>(_WaveStarted);
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);

        playerTransform = GameObject.Find("player").transform;

    }

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        playerTransform = e.activePlayer.transform;
    }

    private void _WaveStarted(WaveStartedEvent e)
    {
        spawnDelay -= 0.1f;

        StartCoroutine(SpawnEnemiesForWave());
    }

    IEnumerator SpawnEnemiesForWave()
    {
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
                //Decide whether to spawn near the player or near the edge of the map
                if (Random.value < 0.3f && playerTransform != null)
                {
                    //Debug.Log("Spawning around player chosen");
                    do
                    {
                        // Decide whether to spawn the enemy on the horizontal (X-axis) or vertical (Z-axis) edges
                        if (Random.value < 0.5f)
                        {
                            // Spawn on the horizontal edges
                            randomX = Random.Range(-spawnDistanceForPlayer, spawnDistanceForPlayer); // Anywhere along the horizontal edge

                            randomZ = Random.Range(spawnDistanceForPlayer, noSpawnZoneDistanceForPlayer);

                            // Top or Bottom edge
                            if (Random.value < 0.5) randomZ *= -1;
                        }
                        else
                        {
                            // Spawn on the vertical edges
                            randomZ = Random.Range(-spawnDistanceForPlayer, spawnDistanceForPlayer); // Anywhere along the vertical edge

                            randomX = Random.Range(spawnDistanceForPlayer, noSpawnZoneDistanceForPlayer);

                            // Top or Bottom edge
                            if (Random.value < 0.5) randomX *= -1;
                        }

                        randomSpawnPosition = new Vector3(playerTransform.position.x + randomX, 10f, playerTransform.position.z + randomZ);
                        //Debug.Log($"Checking to see of the chosen spawn position ({randomSpawnPosition}) is valid");

                    } while (!Physics.Raycast(randomSpawnPosition, Vector3.down, out hitInfo, Mathf.Infinity) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));
                } else
                {
                    //Debug.Log("Spawning at map edge chosen");
                    do
                    {
                        // Decide whether to spawn the enemy on the horizontal (X-axis) or vertical (Z-axis) edges
                        if (Random.value < 0.5f)
                        {
                            // Spawn on the horizontal edges
                            randomX = Random.Range(-spawnDistanceForMap, spawnDistanceForMap); // Anywhere along the horizontal edge

                            randomZ = Random.Range(spawnDistanceForMap, noSpawnZoneDistanceForMap);

                            // Top or Bottom edge
                            if (Random.value < 0.5) randomZ *= -1;
                        }
                        else
                        {
                            // Spawn on the vertical edges
                            randomZ = Random.Range(-spawnDistanceForMap, spawnDistanceForMap); // Anywhere along the vertical edge

                            randomX = Random.Range(spawnDistanceForMap, noSpawnZoneDistanceForMap);

                            // Top or Bottom edge
                            if (Random.value < 0.5) randomX *= -1;
                        }

                        randomSpawnPosition = new Vector3(transform.position.x + randomX, 10f, transform.position.z + randomZ);
                        //Debug.Log($"Checking to see of the chosen spawn position ({randomSpawnPosition}) is valid");

                    } while (!Physics.Raycast(randomSpawnPosition, Vector3.down, out hitInfo, Mathf.Infinity) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));
                }

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
                        spawnHorde(Mathf.Max(4, Mathf.RoundToInt(waveManager.getNumEnemiesToSpawnThisRound() / 6f)), Mathf.Max(3f, waveManager.getWaveNumber() / 2f), randomSpawnPosition);
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
            //Spawn the correct enemy type at the specified position
            if (type is EnemyType.Basic)
            {
                GameObject enemy = Instantiate(BasicEnemyPrefab, position, Quaternion.identity);
            }
            else if (type is EnemyType.Armored)
            {
                GameObject enemy = Instantiate(ArmoredEnemyPrefab, position, Quaternion.identity);
            }
            //let the wave manager know an enemy spawned
            waveManager.enemySpawned(type);
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
