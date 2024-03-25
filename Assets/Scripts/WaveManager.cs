using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{

    private int waveNumber = 1;
    private int maxEnemiesAtOnce = 24;
    private int currentEnemiesAlive = 0;
    private int numEnemiesToSpawnThisRound = 5;
    private int numEnemiesSpawnedSoFar = 0;

    private void Start()
    {
        EventBus.Subscribe<ObjectDestroyedEvent>(_DidEnemyDie);


        //Just for testing, immediately start the wave
        EventBus.Publish(new WaveStartedEvent());
    }

    public void setMaxEnemiesAtOnce(int newNumEnemies)
    {
        maxEnemiesAtOnce = newNumEnemies;
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

    public void enemySpawned()
    {
        numEnemiesSpawnedSoFar++;
        currentEnemiesAlive++;

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






}
