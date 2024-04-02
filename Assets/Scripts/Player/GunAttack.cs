using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAttack : MonoBehaviour
{
    public AmmoSystem ammo;
    public ProjectileBehavior ProjectilePrefab;

    private float firingOffset = 0.5f;

    void Awake()
    {
        EventBus.Subscribe<AttackEvent>(_Attack);
    }
    void _Attack(AttackEvent e)
    {
        if (ammo.ammo_count > 0 && (GetComponentInParent<HandInventory>() != null) && !GetComponentInParent<HandInventory>().knife)
        {
            Vector2 aimDirection = new Vector2(transform.forward.x, transform.forward.z);
            Vector3 spawnPosition = transform.position + transform.forward * firingOffset;

            float spawnAngle = Mathf.Atan2(-aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, spawnAngle, 0f);


            GameObject projectileObject = Instantiate(ProjectilePrefab.gameObject, spawnPosition, rotation);
            //Debug.Log("Gun Attacking");
            EventBus.Publish<ShootEvent>(new ShootEvent());
        }
    }
}
