using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Rendering.CameraUI;
using Weapon.Types;

public class Sword : Weapons
{

    private SpriteRenderer spriteRenderer;
    private BoxCollider hitBox;
    private bool swingingSwordCoRoutine = false;
    private float swingDuration = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }
        hitBox = GetComponent<BoxCollider>();

        EventBus.Subscribe<MainWeaponUsedEvent>(UseWeapon);
    }

    /*
    public override void Use()
    {
        Weapons.currentProjectile = ProjectileType.Sword;

        if (!swingingSwordCoRoutine)
        {
            playerTransform.GetComponent<Rigidbody>().velocity = Vector3.zero;
            swingingSwordCoRoutine = true;
            transform.position = playerTransform.position;
            StartCoroutine(SwingSword(lastMovementDirection));
        }
    }
    */

    public void UseWeapon(MainWeaponUsedEvent e)
    {
        Debug.Log("Using Sword");
        if (!swingingSwordCoRoutine && e.weaponType == WeaponType.Sword)
        {
            swingingSwordCoRoutine = true;

            transform.position = e.playerTransform.position;

            // Get the 2D aim direction
            Vector2 aimDirection2D = e.aimDirection;

            // Calculate the angle from the right (positive X axis) to the aimDirection
            float angle = Mathf.Atan2(aimDirection2D.y, -aimDirection2D.x) * Mathf.Rad2Deg;

            // Rotate around the Y axis to point the sword in the right direction
            Quaternion targetRotation = Quaternion.Euler(90f, angle - 45, 90f);
            transform.rotation = targetRotation;

            StartCoroutine(SwingSword());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy") {
            other.GetComponent<EnemyDamageAndHealth>().TakeDamage(1);
        }
    }

    IEnumerator SwingSword()
    {
        hitBox.enabled = true;
        spriteRenderer.enabled = true;

        // Start rotating the sword
        yield return StartCoroutine(RotateOverTime(Vector3.up * 90, swingDuration));

        // Disable the hitbox and make the sword invisible again
        hitBox.enabled = false;
        spriteRenderer.enabled = false;

        swingingSwordCoRoutine = false;
    }

    IEnumerator RotateOverTime(Vector3 byAngles, float inTime)
    {
        // Store the original rotation of the object
        Quaternion fromAngle = transform.rotation;

        // Calculate the target rotation
        Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);

        // Time passed for interpolation
        for (float t = 0f; t <= 1f; t += Time.deltaTime / inTime)
        {
            // Interpolate the rotation over time
            transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);

            // Wait until next frame
            yield return null;
        }

        // Ensure the rotation is exactly the target rotation
        transform.rotation = toAngle;
    }

}