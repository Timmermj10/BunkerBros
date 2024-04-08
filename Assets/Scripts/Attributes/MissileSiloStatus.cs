using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSiloStatus : MonoBehaviour
{

    private bool isLoaded = false;
    
    public Material loadedMaterial;
    public Material unloadedMaterial;

    public bool isSiloLoaded()
    {
        return isLoaded;
    }

    public void loadSilo()
    {
        isLoaded = true;
        Debug.Log("Loaded Silo");

        //Change visual to indicate silo is loaded
        // Renderer renderer = GetComponent<Renderer>();

        // Change the material of the Renderer to the loaded material
        // renderer.material = loadedMaterial;
    }

    public void unloadSilo()
    {
        isLoaded = false;

        //Change visual to indicate silo is unloaded
        // Renderer renderer = GetComponent<Renderer>();

        // Change the material of the Renderer to the loaded material
        // renderer.material = unloadedMaterial;
    }
}
