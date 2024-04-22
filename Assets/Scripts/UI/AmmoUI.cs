using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoUI : MonoBehaviour
{
    public GameObject swap_display;
    public Text ammo_display;

    public static GameObject ammo_image;
    public static GameObject knife_image;
    public static GameObject gun_image;

    private GameObject gun;
    private GunAttack gunAttack;
    private Subscription<PurchaseEvent> purchase;
    private Subscription<ObjectDestroyedEvent> death;
/*    private Subscription<WeaponSwapEvent> swap;
*/
    // Start is called before the first frame update
    void Awake()
    {
        // Listen for player death
        death = EventBus.Subscribe<ObjectDestroyedEvent>(_PlayerDeath);
        purchase = EventBus.Subscribe<PurchaseEvent>(_EnableText);
/*        swap = EventBus.Subscribe<WeaponSwapEvent>(_Swap);
*/
        ammo_image = GameObject.Find("AmmoImage");
        knife_image = GameObject.Find("KnifeImage");
        gun_image = GameObject.Find("GunImage");
    }

    // Update is called once per frame
    void Update()
    {
        if (gun && gun.activeSelf)
        {
            if (gunAttack.ammoCount == 0 && gunAttack.magCount == 0) {
                swap_display.SetActive(true);
                ammo_display.text = "empty";
            }
            else {
                swap_display.SetActive(false);
                ammo_display.text = gunAttack.magCount.ToString() + "/" + gunAttack.ammoCount.ToString();
            }
            ammo_image.SetActive(true);
            gun_image.SetActive(true);
        }
        else
        {
            swap_display.SetActive(false);
            ammo_display.text = "";
            ammo_image.SetActive(false);
        }
    }

    void _EnableText(PurchaseEvent p)
    {
        if (p.purchasedItem.itemName == "Gun")
        {
            gun = GameObject.Find("RightHand").transform.Find("gun").gameObject;
            gunAttack = gun.GetComponent<GunAttack>();
        }
    }

    /*void _Swap(WeaponSwapEvent e)
    {
        if (e.trueIsKnife)
        {
            gun = GameObject.Find("RightHand").transform.Find("gun").gameObject.GetComponent<GunAttack>();
        }
    }*/

    // Listen for player death
    public void _PlayerDeath(ObjectDestroyedEvent e)
    {
        if (e.name is "player")
        {
            gun = null;
            gunAttack = null;
            gun_image.SetActive(false);
        }
    }

    public void OnDestroy()
    {
        EventBus.Unsubscribe(purchase);
        EventBus.Unsubscribe(death);
    }
}
