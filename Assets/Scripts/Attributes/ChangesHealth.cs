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

        //Speed up computation: Only tagged objects have health
        if (other.gameObject.CompareTag("Untagged"))
        {
            return;
        }

        //Dont let the player damage the objective
        if (other.gameObject.CompareTag("Objective") && (gameObject.CompareTag("Projectile") || gameObject.name is "Knife"))
        {
            return;
        }

        //Dont let enemies damage each other
        if (gameObject.layer == LayerMask.NameToLayer("Enemy") && other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            return;
        }

        //Debug.Log($"OTS Collision: {gameObject.name} with tag {gameObject.tag} and {other.gameObject.name} with tag {other.gameObject.tag}");

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
