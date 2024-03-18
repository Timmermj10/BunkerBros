using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
    public ProjectileBehavior ProjectilePrefab;
    void Awake()
    {
        EventBus.Subscribe<AttackEvent>(_Attack);
    }
    void _Attack(AttackEvent e)
    {
        Vector2 aimDirection = new Vector2(transform.forward.x, transform.forward.z);
        Vector3 spawnPosition = transform.position;

        float spawnAngle = Mathf.Atan2(-aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0f, spawnAngle, 0f);


        GameObject projectileObject = Instantiate(ProjectilePrefab.gameObject, spawnPosition, rotation);
        Debug.Log("Gun Attacking");
    }
}
