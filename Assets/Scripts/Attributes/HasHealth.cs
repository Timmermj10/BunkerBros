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

    private bool inFirstStageTutorial = false;

    private bool canTakeDamage = true;

    public List<HealthBarScript> healthBars;

    private void Start()
    {
        // Set the current health to the max possible
        if (gameObject.name != "SignalStrength")
        {
            currentHealth = maxHealth;
        }

        // Set the health bar to max health

        if (gameObject.CompareTag("Player"))
        {
            healthBars.Add(GameObject.Find("PlayerHealthBar").GetComponent<HealthBarScript>());
        }
        if(healthBars.Count != 0)
        {
            foreach (HealthBarScript bar in healthBars)
            {
                if (bar != null)
                {
                    bar.SetMaxHealth(maxHealth);
                    bar.SetHealth(currentHealth);
                }
            }
        }

        if (gameObject.CompareTag("Objective"))
        {
            inFirstStageTutorial = true;
            EventBus.Subscribe<FirstTutorialWaveEvent>(_FirstTutorialWaveEnded);
            EventBus.Subscribe<LastWaveOverEvent>(_LastWaveEnded);
        }

    }

    private void _LastWaveEnded(LastWaveOverEvent e)
    {
        Debug.Log("Turning off bunker damage");
        canTakeDamage = false;
    }

    private void _FirstTutorialWaveEnded(FirstTutorialWaveEvent e)
    {
        inFirstStageTutorial = false;
    }

    public void increaseHealth(int health_in)
    {
        currentHealth = Mathf.Min(currentHealth + health_in, maxHealth);

        // Update slider
        if (healthBars.Count != 0)
        {
            foreach (HealthBarScript bar in healthBars)
            {
                bar.SetHealth(currentHealth);
            }
        }
    }

    public void changeHealth(int healthChange)
    {
        // Publish that the objective is being damaged
        if (gameObject.name is "Objective" && healthChange < 0)
        {
            EventBus.Publish<ObjectiveDamagedEvent>(new ObjectiveDamagedEvent());
        }

        //Check if the tower is taking damage, and if it has a shield
        if (GetComponent<Shield>() != null)
        {
            //Debug.Log("Shield");
            GetComponent<Shield>().depleteShield(healthChange);
        }

        if (GetComponent<Shield>() == null || !GetComponent<Shield>().protect) 
        {
            
            //If the objective has tken enough damage in the tutorial, dont let it take more
            if (GetComponent<Shield>() != null && currentHealth < (maxHealth - (RepairKitUse.repair_value + 5)) && inFirstStageTutorial)
            {
                return;
            }

            // Update currentHealth
            if (healthChange < 0 && canTakeDamage) 
            {
                //Debug.Log($"{gameObject.name} taking damage");

                // Publish a damage effect event
                if (gameObject.tag == "Enemy")
                {
                    if (healthChange + armorValue <= 0)
                    {
                        EventBus.Publish<DamageEffectEvent>(new DamageEffectEvent(gameObject, true));

                        //if the enemy is not dying
                        if (currentHealth + Mathf.Min(healthChange + armorValue, 0) > 0)
                        {
                            EventBus.Publish(new zombieDamagedEvent(transform.position));
                        }
                    }
                    // Publish a damage effect event
                    else
                    {
                        EventBus.Publish<DamageEffectEvent>(new DamageEffectEvent(gameObject, false));
                    }
                }

                currentHealth += Mathf.Min(healthChange + armorValue, 0);
            }
            else
            {
                currentHealth = Mathf.Min(currentHealth + healthChange, maxHealth);
            }

            // Update slider
            if (healthBars.Count != 0)
            {
                foreach (HealthBarScript bar in healthBars)
                {
                    bar.SetHealth(currentHealth);
                }
            }

            // If we are taking damage
            if (healthChange < 0 && gameObject.CompareTag("Player"))
            {
                EventBus.Publish(new PlayerDamagedEvent(currentHealth));
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

        if (gameObject.name is "player" || gameObject.CompareTag("Pickup")) EventBus.Publish(new InteractTimerEndedEvent());

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

        foreach (HealthBarScript bar in healthBars)
        {
            bar.SetMaxHealth(maxHealth);
        }      
    }
}

