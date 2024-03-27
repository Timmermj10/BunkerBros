using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGameScript : MonoBehaviour
{
    public void OnGameRestart()
    {
        SceneManager.LoadSceneAsync("BalanceScene");
    }
}
