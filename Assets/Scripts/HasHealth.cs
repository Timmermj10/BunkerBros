using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HasHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public HealthBarScript healthBar;

    private void Start()
    {
        // Set the current health to the max possible
        currentHealth = maxHealth;

        // Set the health bar to max health
        if (healthBar != null )
        {
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    public void changeHealth(int healthChange)
    {
        // Update currentHealth
        currentHealth += healthChange;

        // Update slider
        if (healthBar != null )
        {
            healthBar.SetHealth(currentHealth);
        }

        // If we have less than 0 health, DIE
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        EventBus.Publish(new ObjectDestroyedEvent(gameObject.name, gameObject.tag, gameObject.transform.position));

        //Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }


    /*
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            TakeDamage(1);
            StartCoroutine(DamageCooldown(3f)); // Start the cooldown coroutine
        }
    }

    private IEnumerator DamageCooldown(float cooldownTime)
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(cooldownTime);
        canTakeDamage = true;
    }
    */
}

