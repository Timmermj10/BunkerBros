using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandInventory : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventBus.Subscribe<PurchaseEvent>(_Purchase);
    }

    void _Purchase(PurchaseEvent e)
    {
        if(e.purchasedItem.itemName == "Gun")
        {
            transform.Find("Gun").gameObject.SetActive(true);
            transform.Find("Knife").gameObject.SetActive(false);
        }
    }
}
