using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HandInventory : MonoBehaviour
{
    public bool knife = true;
    public bool canSwap = false;
    private KBMandController kb;
    private InputAction swap;
    void Awake()
    {
        EventBus.Subscribe<PurchaseEvent>(_Purchase);
        kb = new KBMandController();

        swap = kb.ActivePlayer.Swap;
        swap.performed += swapWeapons;
    }

    void _Purchase(PurchaseEvent e)
    {
        if(e.purchasedItem.itemName == "Gun")
        {
            Debug.Log("here2");
            knife = false;
            canSwap = true;
            transform.Find("Gun").gameObject.SetActive(true);
            transform.Find("Knife").gameObject.SetActive(false);
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
            }
            else
            {
                transform.Find("Knife").gameObject.SetActive(true);
                transform.Find("Gun").gameObject.SetActive(false);
                knife = true;
            }
        }

    } 
}