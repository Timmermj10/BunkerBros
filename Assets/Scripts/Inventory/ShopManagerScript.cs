using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerScript : MonoBehaviour
{
    public List<InventoryItem> shopItems;
    public float gold;
    public Text goldText;
    private Subscription<CoinCollect> gold_in;

    void Start()
    {
        goldText.text = "Gold: " + gold;
        gold_in = EventBus.Subscribe<CoinCollect>(_gold);
    }

    private void _gold(CoinCollect c)
    {
        gold += c.value;
        goldText.text = "Gold: " + gold;
    }

    // DON'T THINK BUY IS NECESSARY AFTER REWORK
    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        
        if (gold >= shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCost)
        {
            // Take away coins
            gold -= shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCost;

            // Give item
            shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemCount++;

            // Update coins text
            goldText.text = "Gold: " + gold;

            // Update quantity text
            ButtonRef.GetComponent<ButtonInfo>().quantityText.text = shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();

            // Publish purchase event
            EventBus.Publish<PurchaseEvent>(new PurchaseEvent(shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID]));
            Debug.Log(shopItems[ButtonRef.GetComponent<ButtonInfo>().itemID].itemName);
        }
    }
}
