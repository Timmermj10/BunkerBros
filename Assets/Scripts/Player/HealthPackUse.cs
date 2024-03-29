using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthPackUse : MonoBehaviour
{
    public int health_up = 5;

    private HasHealth health;
    private ActivePlayerInventory inv;
    private KBMandController kb;
    private InputAction heal;


    // Start is called before the first frame update
    void Awake()
    {
        health = GetComponent<HasHealth>();
        inv = GetComponent<ActivePlayerInventory>();
        kb = new KBMandController();

        heal = kb.ActivePlayer.UseHeal;
        heal.performed += usePack;
    }

    private void OnEnable()
    {
        heal.Enable();
    }

    private void OnDisable()
    {
        heal.Disable();
    }

    public void usePack(InputAction.CallbackContext context)
    {
        Debug.Log("Button Press");
        if (health != null && inv.itemInInventory(ActivePlayerInventory.activePlayerItems.HealthPack))
        {
            inv.useItem(ActivePlayerInventory.activePlayerItems.HealthPack);
            health.increaseHealth(health_up);
        } else
        {
            Debug.Log("No Health Packs!");
        }
    }
}
