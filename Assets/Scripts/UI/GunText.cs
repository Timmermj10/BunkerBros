using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunText : MonoBehaviour
{
    // Only show the text the first time the players buy the gun
    public bool first = true;
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to purchase events
        EventBus.Subscribe<PurchaseEvent>(_gunUse);
    }

    public void _gunUse(PurchaseEvent e)
    {
        if (e.purchasedItem.itemId == 3 && first)
        {
            first = false;
            EventBus.Publish(new PopUpStartEvent("Player", "TRIANGLE to swap weapons\n LT to ADS", true, true));
        }
    }
}
