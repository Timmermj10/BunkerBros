using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWaveSpawnManager : MonoBehaviour
{
    //Enemy type prefabs
    public GameObject BasicEnemyPrefab;
    public GameObject ArmoredEnemyPrefab;

    //Reference to the pingManager to send prewave pings
    private PingManager pingManager;

    //List of spawnpoints
    private List<Vector3> spawnpoints = new List<Vector3>();

    //Final Wave variables
    private bool isFinalWave = false;
    private float prepTimeForFinalWave = 10f;
    private bool finalWaveOver = false;
    private float finalWaveTimer = 0f;
    private float finalWaveDuration = 120f; // Should be 120
    private float finalWavespawnDelay = 2.5f;
    private float finalWaveSpawnTimer = 0;
    public AnimationCurve finalWaveSpawnCurve;

    //Time between enemy spawned
    private float spawnDelay = 2f;
    private float spawnTimer = 2f;

    //Enemy spawn offsets
    private float randomX;
    private float randomZ;

    //how far away enemies can spawn from the spawnpoint
    private float spawnDistance = 4f;
    private int spawnIndex = 0;
    private List<Vector3> spawnpointsForWave = new List<Vector3>();

    private bool waveOver = false;

    //Wave manager reference to get wave information
    WaveManager waveManager;


    void Start()
    {
        // Set the initial delay
        spawnTimer = spawnDelay;

        waveManager = FindAnyObjectByType<WaveManager>();

        EventBus.Subscribe<WaveStartedEvent>(_WaveStarted);
        EventBus.Subscribe<WaveEndedEvent>(_WaveEnded);
        EventBus.Subscribe<ObjectDestroyedEvent>(_boulderDestroyed);
        EventBus.Subscribe<LastWaveEvent>(_finalWave);
        EventBus.Subscribe<LastWaveOverEvent>(_finalWaveOver);

        spawnpoints.Add(new Vector3(-24, -0.2f, 5)); //Middle Left
        spawnpoints.Add(new Vector3(28, 0.35f, -14)); //Right Middle
        spawnpoints.Add(new Vector3(-1, 0.15f, 35)); //Top Middle

        pingManager = GameObject.Find("GameManager").GetComponent<PingManager>();

    }

    private void _finalWaveOver(LastWaveOverEvent e)
    {
        finalWaveOver = true;
    }

    private void _boulderDestroyed(ObjectDestroyedEvent e)
    {
        //if (e.tag == "Boulder")
        //{
        //    Debug.Log($"location = {e.deathCoordinates}");
        //}

        if (e.tag == "Boulder" && Vector3.Distance(e.deathCoordinates, new Vector3(-3.9f, 0.94f, -20.3f)) < 1)
        {
            //Debug.Log($"Adding {new Vector3(-18, 0.8f, -34)}, Bottom Middle");
            spawnpoints.Add(new Vector3(-18, 0.8f, -34)); //Bottom Middle
        }
        if (e.tag == "Boulder" && Vector3.Distance(e.deathCoordinates, new Vector3(-27.9f, 1.25f, -21.9f)) < 1)
        {
            //Debug.Log($"Adding {new Vector3(-34f, 0.6f, -28f)}, Bottom Left");
            spawnpoints.Add(new Vector3(-34f, 0.6f, -28f)); //Bottom Left
        }
        if (e.tag == "Boulder" && Vector3.Distance(e.deathCoordinates, new Vector3(-20.5f, 1.25f, 31.7f)) < 1)
        {
            //Debug.Log($"Adding {new Vector3(-36, -0.5f, 33)}, Top Left");
            spawnpoints.Add(new Vector3(-36, -0.5f, 33)); //Top Left
        }
        if (e.tag == "Boulder" && (Vector3.Distance(e.deathCoordinates, new Vector3(23.1f, 1.25f, 36.1f)) < 1 || Vector3.Distance(e.deathCoordinates, new Vector3(34.7f, 1.25f, 17.3f)) < 1) && !spawnpoints.Contains(new Vector3(35, 0.525f, 25)))
        {
            //Debug.Log($"Adding {new Vector3(35, 0.525f, 25)}, Top Right");
            spawnpoints.Add(new Vector3(35, 0.525f, 25)); //Top Right
        }
        if (e.tag == "Boulder" && Vector3.Distance(e.deathCoordinates, new Vector3(19f, 1.25f, -28.4f)) < 1)
        {
            //Debug.Log($"Adding {new Vector3(24, 0.15f, -35)}, Bottom Right");
            spawnpoints.Add(new Vector3(24, 0.15f, -35)); //Bottom Right
        }
    }

    private void _WaveEnded(WaveEndedEvent e)
    {
        waveOver = true;
        StartCoroutine(_updateSpawnpoints());
        
    }

    private IEnumerator _updateSpawnpoints()
    {
        yield return new WaitForEndOfFrame();

        //Get the number of spawnpoints
        //Every three waves, the zombies will spawn form an additional lane
        int numSpawnpoints = 1;

        if (waveManager.getWaveNumber() >= 4) numSpawnpoints++;
        if (waveManager.getWaveNumber() >= 7) numSpawnpoints++;
        if (waveManager.getWaveNumber() >= 9) numSpawnpoints++;

        numSpawnpoints = Mathf.Min(spawnpoints.Count, numSpawnpoints);

        spawnpointsForWave.Clear();

        //Pick Random spawnpoints
        for (int i = 0; i < numSpawnpoints; i++)
        {
            spawnIndex = Random.Range(0, spawnpoints.Count);
            while (spawnpointsForWave.Contains(spawnpoints[spawnIndex]))
            {
                spawnIndex = Random.Range(0, spawnpoints.Count);
            }
            spawnpointsForWave.Add(spawnpoints[spawnIndex]);
        }

        //For each spawnpoint ping the starting location
        for (int i = 0; i < spawnpointsForWave.Count; i++)
        {
            pingManager.Ping(spawnpointsForWave[i], 30, PingType.ENEMY);
            //Debug.Log($"Wave Ended, spawn position for next wave = {spawnpointsForWave[i]}");
        }

        yield return null;
    }

    private void _WaveStarted(WaveStartedEvent e)
    {
        waveOver = false;
        spawnDelay -= 0.1f;
        if(!isFinalWave) StartCoroutine(SpawnEnemiesForWave());
    }

    IEnumerator SpawnEnemiesForWave()
    {
        //Debug.Log("Spawning Enemies For Wave");
        while (waveManager.getNumEnemiesSpawnedSoFar() < waveManager.getNumEnemiesToSpawnThisRound() && !waveOver)
        {
            //Make sure the maximum amount of enemies is not exceeded and the amount of enemies per wave is not exceeded
            if (spawnTimer <= 0 && waveManager.getNumEnemiesAlive() < waveManager.getMaxEnemiesAliveAtOnce())
            {
                // Reset the spawn timer
                spawnTimer = Random.Range(spawnDelay - 1, spawnDelay + 1);
                //Debug.Log($"Spawn timer reset to {spawnTimer}");


                Vector3 randomSpawnPosition;
                RaycastHit hitInfo;

                //Spawn an enemy at each spawnpoint
                foreach (Vector3 spawnpointForWave in spawnpointsForWave)
                {

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

                        randomSpawnPosition = new Vector3(spawnpointForWave.x + randomX, 30f, spawnpointForWave.z + randomZ);
                        //Debug.Log($"Checking to see of the chosen spawn position ({randomSpawnPosition}) is valid");

                    } while (!Physics.Raycast(randomSpawnPosition, Vector3.down, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Pickup")) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));

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
        if (((waveManager.getNumEnemiesSpawnedSoFar() < waveManager.getNumEnemiesToSpawnThisRound()) || isFinalWave) && !finalWaveOver && waveManager.getNumEnemiesAlive() < waveManager.getMaxEnemiesAliveAtOnce())
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

        if (isFinalWave)
        {
            maxArmoredToSpawnInHorde = (int)(hordeSize / 2.5f);
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

    private void _finalWave(LastWaveEvent e)
    {
        isFinalWave = true;
        StartCoroutine(FinalWave());
    }


    private IEnumerator FinalWave()
    {
        float progress = 0;
        finalWaveTimer += Time.deltaTime;
        finalWaveSpawnTimer = finalWavespawnDelay;

        int numFinalWaveSpawnpoints = spawnpoints.Count;

        List<Vector3> finalWaveSpawnpoints = new List<Vector3>();

        //Pick Random spawnpoints
        for (int i = 0; i < numFinalWaveSpawnpoints; i++)
        {
            finalWaveSpawnpoints.Add(spawnpoints[i]);
        }

        //For each spawnpoint ping the starting location
        for (int i = 0; i < finalWaveSpawnpoints.Count; i++)
        {
            pingManager.Ping(finalWaveSpawnpoints[i], 60, PingType.ENEMY);
        }

        yield return new WaitForSeconds(prepTimeForFinalWave);

        while (!finalWaveOver)
        {

            if (finalWaveSpawnTimer <= 0)
            {
                progress = finalWaveTimer / finalWaveDuration;

                //Decrease time until next spawn (2.5 is initial spawnDelay)
                finalWavespawnDelay = 2.5f * (1 - finalWaveSpawnCurve.Evaluate(progress)) + 0.2f;

                // Reset the spawn timer
                finalWaveSpawnTimer = finalWavespawnDelay;
                //Debug.Log($"Spawn timer reset to {spawnTimer}");


                Vector3 randomSpawnPosition;
                RaycastHit hitInfo;

                //Spawn an enemy at each spawnpoint
                foreach (Vector3 spawnpointForWave in finalWaveSpawnpoints)
                {
                    if (Random.value < 0.3f) continue;

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

                        randomSpawnPosition = new Vector3(spawnpointForWave.x + randomX, 30f, spawnpointForWave.z + randomZ);
                        //Debug.Log($"Checking to see of the chosen spawn position ({randomSpawnPosition}) is valid");

                    } while (!Physics.Raycast(randomSpawnPosition, Vector3.down, out hitInfo, Mathf.Infinity, ~LayerMask.GetMask("Pickup")) || hitInfo.collider.gameObject.layer != LayerMask.NameToLayer("Default"));

                    randomSpawnPosition.y = hitInfo.point.y + 0.5f;

                    //Decide what type of zombie to spawn

                    if (progress > 0.85f)
                    {
                        StartCoroutine(spawnImpossibleHorde(finalWaveSpawnpoints));
                    }

                    //Scale armored probability by 1.8x progress
                    float armoredEnemyChance = Mathf.Min(1.8f * progress, 0.75f);

                    if (Random.value < armoredEnemyChance)
                    {

                        //Chance to spawn a horde
                        if (Random.value <= 0.3f)
                        {
                            //Spawn a horde of size 10, and radius 4 at the randomSpawnPosition
                            spawnHorde(10, 4, randomSpawnPosition);
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
            }
            else
            {
                // Decrement spawn timer
                finalWaveSpawnTimer -= Time.deltaTime;
            }

            yield return new WaitForFixedUpdate();
        }

        yield return new WaitForFixedUpdate();

    }


    private IEnumerator spawnImpossibleHorde(List<Vector3> impossibleHordeSpawnpoints)
    {
        for (int i = 0; i < 5; i++)
        {
            if (!finalWaveOver)
            {
                foreach (Vector3 spawnpoint in impossibleHordeSpawnpoints)
                {
                    spawnHorde(10, 8, spawnpoint);
                }
            }
            yield return new WaitForSeconds(0.5f);
        }

    }
}
