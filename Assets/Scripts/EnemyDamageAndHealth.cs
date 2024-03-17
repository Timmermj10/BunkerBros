using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageAndHealth : MonoBehaviour
{

    public int health = 1;

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Touched " + collision.collider.tag);
        if (collision.collider.tag == "Player" || collision.collider.tag == "Objective" || collision.collider.tag == "Structure")
        {
            //Deal damage
            DealDamage(collision);
            
            
        } //else if sword or weapon? or have them call takedamage? < call is probably easier
        else if (collision.collider.tag == "Projectile")
        {
            TakeDamage(1); //temp, may change into projectile behavior
        }
    }

    public void DealDamage(Collision c)
    {
        //Decrement health of player, structure, or objective > probably in has health component
        GameObject collide = c.gameObject;
    }

    public void TakeDamage(int damage)
    {
        //Take damage
        health -= damage;
        if (health <= 0)
        {
            EnemyDefeated();
        }
    }

    private void EnemyDefeated()
    {
        Vector3 loca = transform.position;
        //For Spawning resource pickup, and maybe spawner/respawn
        EventBus.Publish<EnemyDefeat>(new EnemyDefeat(loca));

        //Delete or reset
        //Defaulting to Delete for now
        Destroy(gameObject);
    }
}

public class EnemyDefeat
{
    public Vector3 spawn_location;

    public EnemyDefeat(Vector3 location)
    {
        spawn_location = location;
    }
}