using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivePlayerInventory : MonoBehaviour
{

    public enum activePlayerItems
    {
        MissileParts
    }

    private List<activePlayerItems> playerInventory = new List<activePlayerItems>();

    void Start()
    {
        playerInventory.Clear();
        EventBus.Subscribe<PickUpEvent>(_OnPickup);       
    }

    private void _OnPickup(PickUpEvent e)
    {
        //Debug.Log($"Adding {e.pickedUpItem} of type {e.pickedUpItem.GetType()} to activePlayerInventory");
        playerInventory.Add(e.pickedUpItem);
    }

    public bool itemInInventory(activePlayerItems itemName)
    {
        return playerInventory.Contains(itemName);
    }

}
