using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private int waveNumber = 1;
    [SerializeField]
    private int maxEnemiesAtOnce = 24;
    [SerializeField]
    private int currentEnemiesAlive = 0;
    [SerializeField]
    private int numEnemiesToSpawnThisRound = 12; //Initialized to 8 for 8 zombies on round 1
    [SerializeField]
    private int numEnemiesSpawnedSoFar = 0;

    [SerializeField]
    private int numArmoredSpawnedSoFar = 0;
    [SerializeField]
    private int numArmoredToSpawnThisRound = 0;


    [SerializeField]
    private int numHordesSpawnedSoFar = 0;
    [SerializeField]
    private int numHordesToSpawnThisRound = 0;

    private void Start()
    {
        EventBus.Subscribe<ObjectDestroyedEvent>(_DidEnemyDie);
        EventBus.Subscribe<WaveEndedEvent>(_WaveEnd);
    }

    public void setMaxEnemiesAtOnce(int newNumEnemies)
    {
        maxEnemiesAtOnce = newNumEnemies;
    }

    public int getWaveNumber()
    {
        return waveNumber;
    }

    public int getMaxEnemiesAliveAtOnce()
    {
        return maxEnemiesAtOnce;
    }

    public int getNumEnemiesToSpawnThisRound()
    {
        return numEnemiesToSpawnThisRound;
    }

    public int getNumEnemiesSpawnedSoFar()
    {
        return numEnemiesSpawnedSoFar;
    }

    public int getNumEnemiesAlive()
    {
        return currentEnemiesAlive;
    }

    public int getNumArmoredSpawnedSoFar()
    {
        return numArmoredSpawnedSoFar;
    }

    public int getNumArmoredToSpawnThisRound()
    {
        return numArmoredToSpawnThisRound;
    }

    public int getNumHordesSpawnedSoFar()
    {
        return numHordesSpawnedSoFar;
    }

    public int getNumHordesToSpawnThisRound()
    {
        return numHordesToSpawnThisRound;
    }

    public void enemySpawned(EnemyType type)
    {

        if (type == EnemyType.Armored)
        {
            numArmoredSpawnedSoFar++;
        }

       
        numEnemiesSpawnedSoFar++;
        currentEnemiesAlive++;
        //Debug.Log($"Enemy of type: {type} spawned. Enemies spawned this round = {numEnemiesSpawnedSoFar}");

        if (numEnemiesSpawnedSoFar == numEnemiesToSpawnThisRound)
        {
            StartCoroutine(WaitForRoundEnd());
        }
    }

    private IEnumerator WaitForRoundEnd()
    {

        while (currentEnemiesAlive > 0)
        {
            yield return new WaitForEndOfFrame();
        }

        EventBus.Publish(new WaveEndedEvent());
    }



    private void _DidEnemyDie(ObjectDestroyedEvent e)
    {
        if (e.tag is "Enemy")
        {
            currentEnemiesAlive--;
        }
    }

    public void StartWave()
    {
        //Reset the spawn count
        numEnemiesSpawnedSoFar = 0;
        currentEnemiesAlive = 0;

        // Publish the start wave event
        EventBus.Publish(new WaveStartedEvent());
    }

    public void _WaveEnd(WaveEndedEvent e)
    {
        // Increment the wave number
        Debug.Log($"Incrementing WaveNumber to {waveNumber}");
        waveNumber += 1;

        // Increase the number of enemies to be spawned
        numEnemiesToSpawnThisRound =  Mathf.RoundToInt(1.8f * (-0.006f * Mathf.Pow(waveNumber, 3) + 0.4f * Mathf.Pow(waveNumber, 2) + 0.8f * waveNumber + 8f));
        Debug.Log($"Changed numEnemiesToSpawnThisRound to {numEnemiesToSpawnThisRound}");

        // Increase the number of Armored Enemies to be spawned
        numArmoredToSpawnThisRound = Mathf.RoundToInt(2.3f * ((0.02f * waveNumber - 0.025f) * (-0.006f * Mathf.Pow(waveNumber, 3) + 0.4f * Mathf.Pow(waveNumber, 2) + 0.8f * waveNumber + 8f) + waveNumber / 3f));
        Debug.Log($"Changed numArmoredToSpawnThisRound to {numArmoredToSpawnThisRound}");

        // Increase the number of Hordes to be spawned
        numHordesToSpawnThisRound = Mathf.Max(0, Mathf.RoundToInt(2.3f * (( 0.02f * waveNumber - 0.035f) * (-0.004f * Mathf.Pow(waveNumber, 3) + 0.27f * Mathf.Pow(waveNumber, 2) + 0.5f * waveNumber + 6f))));
        Debug.Log($"Changed numHordesToSpawnThisRound to {numHordesToSpawnThisRound}");

    }

}

public enum EnemyType
{
    Basic,
    Armored
}
