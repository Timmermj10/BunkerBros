using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using UnityEngine;
using UnityEngine.InputSystem;

public class GunAttack : MonoBehaviour
{
    private AmmoSystem ammo;
    public ProjectileBehavior ProjectilePrefab;
    public Animator anim;
    public Transform look;

    private Subscription<AttackEvent> sub;
    private float firingOffset = 0.5f;

    private void Start()
    {
        ammo = GameObject.Find("Ammo").GetComponent<AmmoSystem>();
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe(sub);
    }
    private void OnEnable()
    {
        sub = EventBus.Subscribe<AttackEvent>(_Attack);
    }
    void _Attack(AttackEvent e)
    {
        if (ammo.ammo_count > 0)
        {
            Debug.DrawRay(transform.position + look.forward * firingOffset, look.forward, Color.magenta);
            Debug.Log(look.forward);

            GameObject projectileObject = Instantiate(ProjectilePrefab.gameObject, transform.position + look.forward * firingOffset, Quaternion.LookRotation(look.forward));
            //Debug.Log("Gun Attacking");
            EventBus.Publish<ShootEvent>(new ShootEvent());
        }
    }
}
