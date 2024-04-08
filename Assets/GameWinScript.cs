using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{
    void Start()
    {
        EventBus.Subscribe<LastWaveEvent>(endGame);
    }

    public void endGame(LastWaveEvent e)
    {
        SceneManager.LoadSceneAsync("GameWin");
    }
}
