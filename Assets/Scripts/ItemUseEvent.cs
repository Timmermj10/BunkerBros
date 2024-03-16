using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemUseEvent
{
    public int itemID;

    public ItemUseEvent(int itemUsed)
    {
        itemID = itemUsed;
    }
}
