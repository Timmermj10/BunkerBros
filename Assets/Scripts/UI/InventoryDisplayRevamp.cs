using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryDisplayRevamp : MonoBehaviour
{
    public ActivePlayerInventory.activePlayerItems item_to_count;
    public Text count;
    private int num = 0;
    private ActivePlayerInventory inv;
    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        num = 0;

        if (GameObject.Find("player") != null)
        {
            inv = GameObject.Find("player").GetComponent<ActivePlayerInventory>();
        }
        EventBus.Subscribe<PlayerRespawnEvent>(_PlayerRespawn);

    }

    // Update is called once per frame
    void Update()
    {
        if (inv != null)
        {
            num = inv.countItem(item_to_count);
            // img.color = new Color(img.color.r, img.color.g, img.color.b, 1f);
            count.text = num.ToString();
        } else
        {
            // img.color = new Color(img.color.r, img.color.g, img.color.b, 0f);
            count.text = "0";
        }

    }

    private void _PlayerRespawn(PlayerRespawnEvent e)
    {
        inv = e.activePlayer.GetComponent<ActivePlayerInventory>();
    }


}
