using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class HasHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;
    public int armorValue = 0;

    public HealthBarScript healthBar;

    private void Start()
    {
        // Set the current health to the max possible
        currentHealth = maxHealth;

        // Set the health bar to max health

        if (gameObject.CompareTag("Player"))
        {
            healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBarScript>();
        }
        if(healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        // Set the blood to be transparent
        
    }

    //private void Update()
    //{
    //    if (overlay != null)
    //    {
    //        // Check if the blood is onscreen
    //        if (overlay.color.a > 0 && gameObject.name is "player")
    //        {
    //            durationTimer += Time.deltaTime;
    //            if (durationTimer > duration)
    //            {
    //                // Fade the image
    //                float tempAlpha = overlay.color.a;
    //                tempAlpha -= Time.deltaTime * fadeSpeed;
    //                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
    //            }
    //        }
    //    }
    //}

    public void increaseHealth(int health_in)
    {
        currentHealth = Mathf.Min(currentHealth + health_in, maxHealth);

        // Update slider
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void changeHealth(int healthChange)
    {
        //Check if the tower is taking damage, and if it has a shield
        if (GetComponent<Shield>() != null)
        {
            //Debug.Log("Shield");
            GetComponent<Shield>().depleteShield(healthChange);
        }

        if (GetComponent<Shield>() == null || !GetComponent<Shield>().protect) 
        {
            // Update currentHealth

            if (healthChange < 0) 
            {
                currentHealth += Mathf.Min(healthChange + armorValue, 0);
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth + healthChange, maxHealth);
            }

            // Update slider
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            // If we are taking damage
            if (healthChange < 0 && gameObject.CompareTag("Player"))
            {
                EventBus.Publish(new PlayerDamagedEvent());
            }

            // If we have less than 0 health, DIE
            if (currentHealth <= 0)
            {
                Die();
            }
        }
    }


    private void Die()
    {
        EventBus.Publish(new ObjectDestroyedEvent(gameObject.name, gameObject.tag, gameObject.transform.position));

        //Debug.Log(gameObject.name + " has died!");
        Destroy(gameObject);
    }


    public int getMaxHealth()
    {
        return maxHealth;
    }

    //Only called for enemies when scaling health
    public void setHealth(int newHealth)
    {
        //Debug.Log($"Setting health to {newHealth}");
        maxHealth = newHealth;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        
    }
}

