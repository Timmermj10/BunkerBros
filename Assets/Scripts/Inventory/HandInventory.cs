using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandInventory : MonoBehaviour
{
    public bool canSwap = false;

    private Subscription<PurchaseEvent> purchase;
    private Subscription<EmptyAmmo> empty;

    private RawImage knifeImageRenderer;
    private RawImage gunImageRenderer;
    private Animator anim;
    

    void Awake()
    {
        purchase = EventBus.Subscribe<PurchaseEvent>(_Purchase);
        empty = EventBus.Subscribe<EmptyAmmo>(_EmptyAmmo);
        anim = GetComponentInChildren<Animator>();


        if (AmmoUI.knife_image != null) knifeImageRenderer = AmmoUI.knife_image.GetComponent<RawImage>();
        if (AmmoUI.gun_image != null)  gunImageRenderer = AmmoUI.gun_image.GetComponent<RawImage>();
        if (AmmoUI.gun_image.activeSelf) canSwap = true;

        // Set the color of the knife
        knifeImageRenderer.color = new Color(1f, 1f, 1f, 1f);
    }

    void _Purchase(PurchaseEvent e)
    {
        if(e.purchasedItem.itemName == "Gun")
        {
            canSwap = true;
            gunImageRenderer.color = new Color(1f, 1f, 1f, .13f);
            Swap();
        }
    }

    private void OnSwap(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("swap");
            Swap();
        }
    }

    private void _EmptyAmmo(EmptyAmmo e)
    {
        Swap();
    }

    private void Swap()
    {
        if (canSwap)
        {
            Color temp = gunImageRenderer.color;
            gunImageRenderer.color = knifeImageRenderer.color;
            knifeImageRenderer.color = temp;
            anim.SetBool("gun", !anim.GetBool("gun"));

            EventBus.Publish(new WeaponSwapEvent(!anim.GetBool("gun")));
        }
    }
    private void OnDestroy()
    {
        EventBus.Unsubscribe(purchase);
        EventBus.Unsubscribe(empty);
    }
}