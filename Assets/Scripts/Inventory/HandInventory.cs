using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class HandInventory : MonoBehaviour
{
    public bool knife = true;
    public bool canSwap = false;
    private KBMandController kb;
    private InputAction swap;

    private RawImage knifeImageRenderer;
    private RawImage gunImageRenderer;

    

    void Awake()
    {
        EventBus.Subscribe<PurchaseEvent>(_Purchase);
        kb = new KBMandController();

        swap = kb.ActivePlayer.Swap;
        swap.performed += swapWeapons;

        knifeImageRenderer = GameObject.Find("KnifeImage").GetComponent<RawImage>();
        gunImageRenderer = GameObject.Find("GunImage").GetComponent<RawImage>();
    }

    void _Purchase(PurchaseEvent e)
    {
        if(e.purchasedItem.itemName == "Gun")
        {
            knife = false;
            canSwap = true;
            transform.Find("Gun").gameObject.SetActive(true);
            transform.Find("Knife").gameObject.SetActive(false);
            
            knifeImageRenderer.color = new Color(1f, 1f, 1f, 0.13f);
            gunImageRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private void OnEnable()
    {
        swap.Enable();
    }

    private void OnDisable()
    {
        swap.Disable();
    }

    public void swapWeapons(InputAction.CallbackContext context)
    {
        if (canSwap)
        {

            if (knife)
            {
                transform.Find("Gun").gameObject.SetActive(true);
                transform.Find("Knife").gameObject.SetActive(false);
                knife = false;

                knifeImageRenderer.color = new Color(1f, 1f, 1f, 0.13f);
                gunImageRenderer.color = new Color(1f, 1f, 1f, 1f);
                // Debug.Log(knifeImageRenderer.color);
            }
            else
            {
                transform.Find("Knife").gameObject.SetActive(true);
                transform.Find("Gun").gameObject.SetActive(false);
                knife = true;

                gunImageRenderer.color = new Color(1f, 1f, 1f, 0.13f);
                knifeImageRenderer.color = new Color(1f, 1f, 1f, 1f);
            }
        }

    } 
}