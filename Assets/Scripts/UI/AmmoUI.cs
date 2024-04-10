using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammo_display;
    private GameObject ammo_image;
    public AmmoSystem ammo;
    public bool enable = false;

    private Subscription<PurchaseEvent> purchase;

    // Start is called before the first frame update
    void Awake()
    {
        purchase = EventBus.Subscribe<PurchaseEvent>(_enable_text);
        ammo_display = GameObject.Find("Ammo").GetComponentInChildren<Text>();
        ammo_image = GameObject.Find("AmmoImage");

        if(ammo_display != null)
        {
            // Debug.Log("Ammo UI Text Found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo_display != null)
        {
            if (enable)
            {
                ammo_display.text = ammo.ammo_count.ToString();
            }
            else
            {
                ammo_display.text = "";
                ammo_image.SetActive(false);
            }
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
