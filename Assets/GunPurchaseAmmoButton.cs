using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPurchaseAmmoButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to purchase events
        EventBus.Subscribe<PurchaseEvent>(_gunUse);
    }

    public void _gunUse(PurchaseEvent e)
    {
        if (e.purchasedItem.itemId == 3)
        {
            gameObject.SetActive(true);
        }
    }
}
