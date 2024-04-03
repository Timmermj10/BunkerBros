using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplay : MonoBehaviour
{
    public ActivePlayerInventory.activePlayerItems item_to_count;
    public Text count;
    public int num = 0;
    public ActivePlayerInventory inv;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        num = inv.countItem(item_to_count);
        if (num <= 0)
        {
            count.text = "";
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
        } else
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            count.text = num.ToString();
        }

    }
}
