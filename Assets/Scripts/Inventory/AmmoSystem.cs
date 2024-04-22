using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoSystem : MonoBehaviour
{
    public int mag_size = 8;
    public int mag_count = 8;
    public int ammo_count = 60;
    public int reload_count = 60;

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
        mag_count -= 1;
        if (mag_count == 0)
        {
            EventBus.Publish(new EmptyAmmo());
            mag_count = mag_size;
            ammo_count -= mag_size;
        }
    }
    private void OnDestroy()
    {
        EventBus.Unsubscribe(purchase);
        EventBus.Unsubscribe(pickup);
        EventBus.Unsubscribe(shot);
    }
}
