using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{

    private PopUpSystem popUpSystem;

    private bool managerConfirmed = false;
    private bool playerConfirmed = false;

    void Start()
    {
        EventBus.Subscribe<GameOverEvent>(endGame);
        EventBus.Subscribe<PopUpEndEvent>(_playerConfirmed);
        popUpSystem = GameObject.Find("GameManager").GetComponent<PopUpSystem>();
    }

    public void endGame(GameOverEvent e)
    {
        StartCoroutine(finalScene());
    }

    private IEnumerator finalScene()
    {

        managerConfirmed = false;
        playerConfirmed = false;
        popUpSystem.popUp("Player", "The Extraction Team is here! You have managed to hold out long enough and have successfully escaped from the zombie infested desert! Congratulations!");
        popUpSystem.popUp("Manager", "The Extraction Team is here! You have managed to hold out long enough and have successfully escaped from the zombie infested desert! Congratulations!");

        while (!managerConfirmed || !playerConfirmed)
        {
            yield return new WaitForSeconds(0.1f);
        }

        SceneManager.LoadSceneAsync("GameWin");
        yield return null;
    }

    private void _playerConfirmed(PopUpEndEvent e)
    {
        if (e.player == "Manager")
        {
            managerConfirmed = true;
        } else
        {
            playerConfirmed = true;
        }
    }
}
