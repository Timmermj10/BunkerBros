using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float range = 10f;
    public Transform projectilePrefab;
    public Transform firePoint;
    public LayerMask enemyLayer;

    void Update()
    {
        DetectEnemiesInRange();
    }

    void DetectEnemiesInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, range, enemyLayer);
        if (hitColliders.Length > 0)
        {
            // Transform to hold the closest enemy
            Transform closestEnemy = null;

            // The closest distance
            float closestDistanceSqr = Mathf.Infinity;

            // Current position of the turret
            Vector3 currentPosition = transform.position;

            // Loop through what is within the range
            foreach (Collider hitCollider in hitColliders)
            {
                // Get the direction toward the target
                Vector3 directionToTarget = hitCollider.transform.position - currentPosition;

                // Get the distance squared to target
                float dSqrToTarget = directionToTarget.sqrMagnitude;

                // If the distance squared is less than the current closest
                if (dSqrToTarget < closestDistanceSqr)
                {
                    // Update the variables
                    closestDistanceSqr = dSqrToTarget;
                    closestEnemy = hitCollider.transform;
                }
            }

            // If we have a closest enemy
            if (closestEnemy != null)
            {
                // Aim at the closest enemy (optional)
                AimAtEnemy(closestEnemy);

                // Fire at the closest enemy
                FireAtEnemy(closestEnemy);
            }
        }
    }

    void AimAtEnemy(Transform target)
    {
        // Point the turret towards the enemy
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void FireAtEnemy(Transform target)
    {
        // Instantiate a projectile and set its direction towards the target
        Transform projectileInstance = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        // projectileInstance.GetComponent<Projectile>().SetTarget(target);
    }
}
