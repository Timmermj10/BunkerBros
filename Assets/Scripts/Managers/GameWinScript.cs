using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinScript : MonoBehaviour
{
    void Start()
    {
        EventBus.Subscribe<GameOverEvent>(endGame);
    }

    public void endGame(GameOverEvent e)
    {
        SceneManager.LoadSceneAsync("GameWin");
    }
}
