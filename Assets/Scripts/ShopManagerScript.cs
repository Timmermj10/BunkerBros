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

    void Start()
    {
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
        }
    }
}
