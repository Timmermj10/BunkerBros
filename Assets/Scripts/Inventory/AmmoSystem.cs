using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    public int ammo_count = 10;
    public int reload_count = 10;

    public bool active = false;
    private Subscription<PurchaseEvent> purchase;
    private Subscription<PickUpEvent> pickup;
    private Subscription<ShootEvent> shot;

    private void Awake()
    {
        purchase = EventBus.Subscribe<PurchaseEvent>(_enable);
        pickup = EventBus.Subscribe<PickUpEvent>(_refill);
        shot = EventBus.Subscribe<ShootEvent>(_decrement);
    }

    private void _enable(PurchaseEvent p)
    {
        if (p.purchasedItem.itemName == "Gun")
        {
            active = true;
        }
    }

    private void _refill(PickUpEvent p)
    {
        if (p.pickedUpItem == ActivePlayerInventory.activePlayerItems.AmmoKit)
        {
            ammo_count += reload_count;
        }
    }

    private void _decrement(ShootEvent s)
    {
        ammo_count -= 1;
        if (ammo_count == 0)
        {
            ammo_count = 0;
        }
    }

}
