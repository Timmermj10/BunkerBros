using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsInteractableSilo : MonoBehaviour
{
    // Parent gameObject
    GameObject parent;

    // Get the missile silo status
    MissileSiloStatus status;

    // Get the active player inventory
    ActivePlayerInventory inv;


    // Start is called before the first frame update
    void Start()
    {
        // Get the parent gameObject
        parent = gameObject.transform.parent.gameObject;

        // Get silo status
        status = parent.GetComponent<MissileSiloStatus>();

        if (GameObject.Find("player") != null)
        {
            inv = GameObject.Find("player").GetComponent<ActivePlayerInventory>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GameObject.Find("player") != null)
        {
            inv = GameObject.Find("player").GetComponent<ActivePlayerInventory>();
        }

        // See if we need to update the interactablilty
        // Check that the object is untagged, the silo isn't loaded, and we have a nuke in the inventory
        if (inv != null)
        {
            if (gameObject.tag is "Untagged" && !status.isSiloLoaded() && inv.countItem(ActivePlayerInventory.activePlayerItems.NukeParts) > 0)
            {
                gameObject.tag = "Interactable";
            }
            else if (tag is "Interactable" && status.isSiloLoaded())
            {
                Debug.Log("here");
                gameObject.tag = "Untagged";
            }
        }
    }
}
