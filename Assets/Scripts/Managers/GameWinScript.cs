using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{
    private bool managerConfirmed = false;
    private bool playerConfirmed = false;

    void Start()
    {
        EventBus.Subscribe<GameOverEvent>(endGame);
        EventBus.Subscribe<PopUpEndEvent>(_playerConfirmed);
    }

    public void endGame(GameOverEvent e)
    {
        StartCoroutine(finalScene());
    }

    private IEnumerator finalScene()
    {

        managerConfirmed = false;
        playerConfirmed = false;
        EventBus.Publish(new PopUpStartEvent("Player", "The Extraction Team is here! You have managed to hold out long enough and have successfully escaped from the zombie infested desert! Congratulations!"));
        EventBus.Publish(new PopUpStartEvent("Manager", "The Extraction Team is here! You have managed to hold out long enough and have successfully escaped from the zombie infested desert! Congratulations!"));

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
