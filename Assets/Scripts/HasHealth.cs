using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;
using UnityEngine.UI;

public class HasHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    public HealthBarScript healthBar;

    [Header("Damage Overlay")]
    public Image overlay; // DamageOverlay GameObject
    public float duration; // how long the image will stay
    public float fadeSpeed; // how quickly the red will fade

    private float durationTimer; // timer to check against the duration

    [Header("Damage Overlay")]
    public Image overlay; // DamageOverlay GameObject
    public float duration; // how long the image will stay
    public float fadeSpeed; // how quickly the red will fade

    private float durationTimer; // timer to check against the duration

    private void Start()
    {
        // Set the current health to the max possible
        currentHealth = maxHealth;

        // Set the health bar to max health
        if (healthBar != null )
        {
            healthBar.SetMaxHealth(maxHealth);
        }

        // Set the blood to be transparent
        if (gameObject.name is "player")
        {
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 0);
        }
    }

    private void Update()
    {
<<<<<<< Updated upstream
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
=======
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
>>>>>>> Stashed changes
            }
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

        // If we are taking damage
        if (healthChange < 0 && gameObject.name is "player")
        {
            durationTimer = 0;
            overlay.color = new Color(overlay.color.r, overlay.color.g, overlay.color.b, 1);
        }

        // If we are taking damage
        if (healthChange < 0 && gameObject.name is "player")
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

