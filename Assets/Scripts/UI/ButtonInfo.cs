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
        if (priceText != null)
        {
            priceText.text = "Price: $" + ShopManager.GetComponent<ShopManagerScript>().shopItems[itemID].itemCost.ToString();
        }

        // Update quantity text
        if (quantityText != null)
        {
            quantityText.text = ShopManager.GetComponent<ShopManagerScript>().shopItems[itemID].itemCount.ToString();
        }
    }
}

