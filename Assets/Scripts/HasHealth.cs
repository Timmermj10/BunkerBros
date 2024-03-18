using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class HasHealth : MonoBehaviour
{
    public int maxHealth = 1;
    public int currentHealth;

    private bool canTakeDamage = true;

    private void Start()
    {
        currentHealth = maxHealth;
    }

    public void changeHealth(int healthChange)
    {
        currentHealth += healthChange;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {

        if (gameObject.CompareTag("Enemy"))
        {
            EventBus.Publish<EnemyDefeat>(new EnemyDefeat(transform.position));
        }

        Debug.Log(gameObject.name + " has died!");
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

