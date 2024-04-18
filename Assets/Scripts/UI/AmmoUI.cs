using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    private Text ammo_display;
    public Text swap_display;
    private GameObject ammo_image;
    public static GameObject knife_image;
    public static GameObject gun_image;
    public int current_mag = 30;
    public int reserve_mags;
    private int prev_total;
    public AmmoSystem ammo;
    public bool enable = false;
    
    private Subscription<PurchaseEvent> purchase;

    // Start is called before the first frame update
    void Awake()
    {
        purchase = EventBus.Subscribe<PurchaseEvent>(_enable_text);
        ammo_display = GameObject.Find("Ammo").GetComponentInChildren<Text>();
        ammo_image = GameObject.Find("AmmoImage");
        gun_image = GameObject.Find("GunImage");
        knife_image = GameObject.Find("KnifeImage");

        if(ammo_display != null)
        {
            // Debug.Log("Ammo UI Text Found");
        }

        if (swap_display != null)
        {
            swap_display.text = "";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ammo_display != null)
        {
            if (enable)
            {
                ammo_image.SetActive(true);
                gun_image.SetActive(true);
                if (ammo.ammo_count == 0) {
                    ammo_display.text = "empty";
                    Debug.Log("Ammo is Empty");
                    prev_total = 0;
                    reserve_mags = 0;
                    current_mag = 0;
                    //Adding Swap Text
                    swap_display.text = "SWAP:TRI";
                }
                else {
                    if (ammo.ammo_count > prev_total) {
                        reserve_mags += 60;
                    }
                    current_mag = ammo.ammo_count % 30;
                    if (current_mag == 0) {
                        reserve_mags = ammo.ammo_count - 30;
                        current_mag = 30;
                    }
                    prev_total = ammo.ammo_count;
                }
                swap_display.text = "";
                ammo_display.text = current_mag.ToString() + "/" + reserve_mags.ToString();
            }
            else
            {
                ammo_display.text = "∞";
                if (ammo_image != null) ammo_image.SetActive(false);
                if (gun_image != null) gun_image.SetActive(false);
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
