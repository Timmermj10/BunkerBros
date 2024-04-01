using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float cooldown = .5f;
    public ProjectileBehavior ProjectilePrefab;
    public float activationDistance = 10f;
    private float nextShot;
    private Transform gun;
    private Transform bulletSpawnpoint;

    private bool active = false;

    //Offset from the turret so the turret cant hit itself
    public float firingOffset = 1f;

    private void Awake()
    {
        nextShot = Time.time;
        gun = transform.Find("gun");
        bulletSpawnpoint = gun.Find("BulletSpawnpoint");

        // Delete both these lines when adding turret activation
        active = true; 
        gameObject.tag = "Structure";
    }

    private void FixedUpdate()
    {
        if (active)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            Vector3 closest = Vector3.right * activationDistance;
            foreach (GameObject enemy in enemies)
            {
                Vector3 toEnemy = enemy.transform.position - gun.position;
                if (toEnemy.magnitude < closest.magnitude)
                {
                    closest = toEnemy;
                }
            }
            if (closest.magnitude < activationDistance)
            {
                closest.y = 0;
                gun.LookAt(gun.transform.position + closest);
                if (nextShot < Time.time)
                {
                    Vector2 aimDirection = new Vector2(gun.forward.x, gun.forward.z);
                    Vector3 spawnPosition = bulletSpawnpoint.position + gun.forward * firingOffset;

                    float spawnAngle = Mathf.Atan2(-aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.Euler(0f, spawnAngle, 0f);

                    GameObject projectileObject = Instantiate(ProjectilePrefab.gameObject, spawnPosition, rotation);

                    projectileObject.GetComponent<ChangesHealth>().setHealthChange(-2);
                    nextShot = Time.time + cooldown;
                }
            }
        }
    }

    public void activateTurret()
    {
        gameObject.tag = "Structure";
        active = true;
    }
}
