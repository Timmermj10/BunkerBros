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

        //Debug.Log($"ChangesHealth OTS: {gameObject.name} and {other.gameObject.name}");
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


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Untagged"))
        {
            return;
        }

        //Debug.Log($"ChangesHealth OTE: {gameObject.name} and {other.gameObject.name}");
        //Get the HasHealth Component of the object collided with
        HasHealth hasHealth = other.gameObject.GetComponent<HasHealth>();

        //If the object is a type that has health
        if (hasHealth != null && damageCooldownTimer <= 0)
        {
            //Debug.Log($"OTE: {gameObject.name} is dealing damage to {other.gameObject.name}");
            //Modify damage by healthChange
            hasHealth.changeHealth(healthChange);
            damageCooldownTimer = damageCooldown;
        }
    }


    /*private void OnCollisionStay(Collision collision)
    {

        Debug.Log($"ChangesHealth Collision detected: {gameObject.name} and {collision.gameObject.name}");

        //Get the HasHealth Component of the object collided with
        HasHealth hasHealth = collision.gameObject.GetComponent<HasHealth>();

        //If the object is a type that has health
        if (hasHealth != null && damageCooldownTimer <= 0)
        {
            //Modify damage by healthChange
            hasHealth.TakeDamage(healthChange);
            damageCooldownTimer = damageCooldown;

        }
    }*/

    private void FixedUpdate()
    {
        if (damageCooldownTimer >= 0)
        {
            damageCooldownTimer -= Time.deltaTime;
            
            /*
            if (damageCooldownTimer <= 0)
            {
                Debug.Log($"Damage cooldown reset for {gameObject.name}");
            }*/
        }
    }

}
