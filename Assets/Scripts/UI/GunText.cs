using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunText : MonoBehaviour
{
    // Bool to hold if the text is on screen
    private bool textShowing = false;

    // How long the text will be on screen
    private float timer = 3f;


    // Start is called before the first frame update
    void Start()
    {
        // Subscribe to purchase events
        EventBus.Subscribe<PurchaseEvent>(_gunUse);

        // Set it so they know how to shoot
        gameObject.GetComponent<Text>().text = "Click RIGHT BUMBER to stab/shoot";
        textShowing = true;

    }

    // Update is called once per frame
    void Update()
    {
        // If we are showing the text
        if (textShowing)
        {
            timer -= Time.deltaTime;

            if (timer < 0)
            {
                // Reset the text
                GetComponent<Text>().text = "";
                textShowing = false;
                timer = 3f;
            }
        }
    }

    public void _gunUse(PurchaseEvent e)
    {
        if (e.purchasedItem.itemId == 3)
        {
            // Update bool
            textShowing = true;

            // Set the text
            gameObject.GetComponent<Text>().text = "Click TRIANGLE to switch between weapons";
        }
    }
}
