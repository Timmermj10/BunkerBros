using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairKit : Interactable
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    // Define the interaction
    protected override void Interact()
    {
        Debug.Log($"Interacted with {gameObject.name}");
    }
}
