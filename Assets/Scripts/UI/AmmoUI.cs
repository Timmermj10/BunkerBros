using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public Text ammo_display;
    public AmmoSystem ammo;
    public bool enable = false;

    private Subscription<PurchaseEvent> purchase;

    // Start is called before the first frame update
    void Awake()
    {
        purchase = EventBus.Subscribe<PurchaseEvent>(_enable_text);
    }

    // Update is called once per frame
    void Update()
    {
        if (enable)
        {
            ammo_display.text = "Ammo: " + ammo.ammo_count.ToString();
        } else
        {
            ammo_display.text = "";
        }
    }

    void _enable_text(PurchaseEvent p)
    {
        if (p.purchasedItem.itemName == "Gun")
        {
            enable = true;
        } else
        {
            Debug.Log("Purchased Item was: " + p.purchasedItem.itemName);
        }
        
    }
}
