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
    private int numEnemiesToSpawnThisRound = 5;
    [SerializeField]
    private int numEnemiesSpawnedSoFar = 0;

    [SerializeField]
    private int numArmoredSpawnedSoFar = 0;
    [SerializeField]
    private int numArmoredToSpawnThisRound = 0;

    [Header("Wave Button")]
    public GameObject waveButton;

    private void Start()
    {
        EventBus.Subscribe<ObjectDestroyedEvent>(_DidEnemyDie);
        EventBus.Publish(new WaveEndedEvent());
        EventBus.Subscribe<WaveEndedEvent>(_WaveEnd);
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

    public int getNumArmoredSpawnedSoFar()
    {
        return numArmoredSpawnedSoFar;
    }

    public int getNumArmoredToSpawnThisRound()
    {
        return numArmoredToSpawnThisRound;
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

        // Publish the start wave event
        EventBus.Publish(new WaveStartedEvent());

        // Make the button not interactable
        waveButton.GetComponent<Button>().interactable = false;
    }

    public void _WaveEnd(WaveEndedEvent e)
    {
        // Make the button interactable
        waveButton.GetComponent<Button>().interactable = true;

        // Increment the wave number
        waveNumber += 1;

        // Increase the number of enemies to be spawned
        numEnemiesToSpawnThisRound = Mathf.RoundToInt(numEnemiesToSpawnThisRound * 1.5f);
        //Debug.Log($"Changed numEnemiesToSpawnThisRound to {numEnemiesToSpawnThisRound}");

        // Increase the number of Armored Enemies to be spawned
        numArmoredToSpawnThisRound = Mathf.RoundToInt(numArmoredToSpawnThisRound * 1.6f) + 1;
        //Debug.Log($"Changed numArmoredToSpawnThisRound to {numArmoredToSpawnThisRound}");
    }
}

public enum EnemyType
{
    Basic,
    Armored
}
