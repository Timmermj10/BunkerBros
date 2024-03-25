using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangesHealth : MonoBehaviour
{

    public int healthChange = -1;

    private float damageCooldownTimer = 0f;
    public float damageCooldown = 1f;

    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Untagged"))
        {
            return;
        }

        //if (gameObject.layer == other.gameObject.layer)
        //{
        //    return;
        //}

        //Debug.Log($"OTS Collision: {gameObject.name} and {other.gameObject.name} with damageCooldown of {damageCooldownTimer}");

        //Get the HasHealth Component of the object collided with
        HasHealth hasHealth = other.gameObject.GetComponent<HasHealth>();

        //If the object is a type that has health
        if (hasHealth != null && damageCooldownTimer <= 0)
        {
            //Debug.Log($"OTS: {gameObject.name} is dealing damage to {other.gameObject.name}");
            //Modify damage by healthChange
            hasHealth.changeHealth(healthChange);
            damageCooldownTimer = damageCooldown;
        }
    }


    private void Update()
    {
        if (damageCooldownTimer > 0)
        {
            damageCooldownTimer -= Time.deltaTime;

            //Debug.Log($"DamageCooldownTimer = {damageCooldownTimer}");

            if (damageCooldownTimer <= 0)
            {
                //Debug.Log($"Damage cooldown reset for {gameObject.name}");
            }
        }
    }

    public void setHealthChange(int newHealthChange)
    {
        healthChange = newHealthChange;
    }

}
