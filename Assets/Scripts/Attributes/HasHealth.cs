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

    [Header("Damage Overlay")]
    private Image overlay; // DamageOverlay GameObject
    public float duration; // how long the image will stay
    public float fadeSpeed; // how quickly the red will fade

    private float durationTimer; // timer to check against the duration

    private void Start()
    {
        // Set the current health to the max possible
        currentHealth = maxHealth;

        // Set the health bar to max health

        if (gameObject.CompareTag("Player"))
        {
            healthBar = GameObject.Find("PlayerHealthBar").GetComponent<HealthBarScript>();
            overlay = GameObject.Find("DamageOverlay").GetComponent<Image>();
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
        if(healthBar != null)
            healthBar.SetMaxHealth(maxHealth);

        // Set the blood to be transparent
        
    }

    private void Update()
    {
        if (overlay != null)
        {
            // Check if the blood is onscreen
            if (overlay.color.a > 0 && gameObject.name is "player")
            {
                durationTimer += Time.deltaTime;
                if (durationTimer > duration)
                {
                    // Fade the image
                    float tempAlpha = overlay.color.a;
                    tempAlpha -= Time.deltaTime * fadeSpeed;
                    overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, tempAlpha);
                }
            }
        }
    }

    public void increaseHealth(int health_in)
    {
        currentHealth += health_in;

        if (currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }

        // Update slider
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    public void changeHealth(int healthChange)
    {
        //Check if the tower is taking damage, and if it has a shield
        if (GetComponent<Shield>() != null && GetComponent<Shield>().protect)
        {
            //Debug.Log("Shield");
            GetComponent<Shield>().depleteShield(healthChange);
        }
        else
        {
            // Update currentHealth
            currentHealth += Mathf.Min(healthChange + armorValue, 0);

            // Update slider
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            // If we are taking damage
            if (healthChange < 0 && gameObject.CompareTag("Player"))
            {
                durationTimer = 0;
                overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
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

