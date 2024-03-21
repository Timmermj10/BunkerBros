using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileSiloStatus : MonoBehaviour
{

    private bool isLoaded = false;
    
    public Material loadedMaterial;

    public bool isSiloLoaded()
    {
        return isLoaded;
    }

    public void loadSilo()
    {
        isLoaded = true;

        //Change visual to indicate silo is loaded
        Renderer renderer = GetComponent<Renderer>();

        // Change the material of the Renderer to the loaded material
        renderer.material = loadedMaterial;
    }
}
