using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DetectTower : MonoBehaviour
{
    private void Update()
    {
        // Get a reference to the main tower
        GameObject mainTower = GameObject.Find("Objective");

        // If the main tower is destroyed, change scene to the restart screen
        if (mainTower == null)
        {
            SceneManager.LoadSceneAsync("GameEnd");
        }
    }
}
