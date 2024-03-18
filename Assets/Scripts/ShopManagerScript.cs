using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerScript : MonoBehaviour
{
    public List<InventoryItem> shopItems;
    public float coins;
    public Text coinsText;
    private Subscription<CoinCollect> coin_in;

    void Start()
    {
        coinsText.text = "Coins: " + coins;
        coin_in = EventBus.Subscribe<CoinCollect>(_coin);
    }

    private void _coin(CoinCollect c)
    {
        coins += c.value;
        coinsText.text = "Coins: " + coins;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        
        if (coins >= shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCost)
        {
            // Take away coins
            coins -= shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCost;

            // Give item
            shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCount++;

            // Update coins text
            coinsText.text = "Coins: " + coins;

            // Update quantity text
            ButtonRef.GetComponent<ButtonInfo>().quantityText.text = shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();

            // Publish purchase event
            EventBus.Publish<PurchaseEvent>(new PurchaseEvent(shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID]));
        }
    }
}

// Class for general item purchases
public class PurchaseEvent
{
    public int itemID;
    public string itemName;
    public Sprite itemIcon;
    public int itemCount;
    public bool isOneTimePurchase;

    public PurchaseEvent(InventoryItem item)
    {
        itemID = item.itemId;
        itemName = item.itemName;
        itemIcon = item.itemIcon;
        itemCount = item.itemCount;
        isOneTimePurchase = item.oneTimePurchase;
    }
}