using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    // Message displayed to player when looking at an interactable
    public string promptMessage;

    // Function to be called from the player
    public void BaseInteract()
    {
        Interact();
    }

    protected virtual void Interact()
    {
        // No code
        // This is a template to be overridden by our subclasses
    }
}
