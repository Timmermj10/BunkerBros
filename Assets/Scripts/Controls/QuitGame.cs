using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("GameStart");
        }       
    }

    // When the quit button is clicked, close the application
    public void OnGameQuit()
    {
        Application.Quit();
    }
}
