using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonInfo : MonoBehaviour
{

    public int itemID;
    public Text priceText;
    public Text quantityText;
    public GameObject ShopManager;

    // Update is called once per frame
    void Update()
    {
        // Update price text
        priceText.text = "Price: $" + ShopManager.GetComponent<ShopManagerScript>().shopItems[itemID].itemCost.ToString();

        // Update quantity text
        quantityText.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[itemID].itemCount.ToString();
    }
}

