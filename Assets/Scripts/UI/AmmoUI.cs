using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public AmmoSystem ammo;
    public Text swap_display;
    public Text ammo_display;

    public static GameObject ammo_image;
    public static GameObject knife_image;
    public static GameObject gun_image;

    private GunAttack gun;
    private Subscription<PurchaseEvent> purchase;
    private Subscription<ObjectDestroyedEvent> death;

    // Start is called before the first frame update
    void Awake()
    {
        // Listen for player death
        death = EventBus.Subscribe<ObjectDestroyedEvent>(_PlayerDeath);
        purchase = EventBus.Subscribe<PurchaseEvent>(_EnableText);

        ammo_image = GameObject.Find("AmmoImage");
        knife_image = GameObject.Find("KnifeImage");
        gun_image = GameObject.Find("GunImage");

        if (ammo_display != null)
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
        if (gun)
        {
            ammo_image.SetActive(true);
            gun_image.SetActive(true);
            if (ammo.ammo_count == 0 && ammo.mag_count == 0) {
                ammo_display.text = "empty";
                swap_display.text = "SWAP:TRI";
            }
            else {
                swap_display.text = "";
                ammo_display.text = ammo.mag_count.ToString() + "/" + ammo.ammo_count.ToString();
            }
        }
        else
        {
            ammo_display.text = "∞";
            if (ammo_image != null) ammo_image.SetActive(false);
            if (gun_image != null) gun_image.SetActive(false);
        }
    }

    void _EnableText(PurchaseEvent p)
    {
        if (p.purchasedItem.itemName == "Gun")
        {
            gun = GameObject.Find("gun").GetComponent<GunAttack>();
        }
    }

    // Listen for player death
    public void _PlayerDeath(ObjectDestroyedEvent e)
    {
        if (e.name is "player")
        {
            gun = null;
        }
    }

    public void OnDestroy()
    {
        EventBus.Unsubscribe(purchase);
        EventBus.Unsubscribe(death);
    }
}
