using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Types;

public class MainWeaponUsedEvent
{
    public WeaponType weaponType = WeaponType.Sword;
    public Vector2 aimDirection = Vector2.zero;
    public Transform playerTransform = null;

    public MainWeaponUsedEvent(WeaponType _newWeaponType, Vector2 _newAimDirection, Transform _newPlayerTransform)
    {
        weaponType = _newWeaponType;
        aimDirection = _newAimDirection;
        playerTransform = _newPlayerTransform;
    }


}

