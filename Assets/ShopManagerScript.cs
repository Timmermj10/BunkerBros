using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopManagerScript : MonoBehaviour
{
    public int[,] shopItems = new int[4,4];
    public float coins;
    public Text coinsText;

    void Start()
    {
        coinsText.text = "Coins: " + coins;

        // Shop IDs
        shopItems[1, 1] = 1;
        shopItems[1, 2] = 2;
        shopItems[1, 3] = 3;

        // Price
        shopItems[2, 1] = 50;
        shopItems[2, 2] = 25;
        shopItems[2, 3] = 150;

        // Quantity & Inventory
        shopItems[3, 1] = 0;
        shopItems[3, 2] = 0;
        shopItems[3, 3] = 0;
    }

    public void Buy()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;

        if (coins >= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID])
        {
            // Take away coins
            coins -= shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID];

            // Give item
            shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;

            // Update coins text
            coinsText.text = "Coins: " + coins;

            // Update quantity text
            ButtonRef.GetComponent<ButtonInfo>().quantityText.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();
        }
    }
}
