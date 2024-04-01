using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameScript : MonoBehaviour
{
    // Load the main scene when the start button is clicked
    public void OnGameStart()
    {
        SceneManager.LoadSceneAsync("StoryScene");
    }
}
