using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Weapon.Types;

namespace Weapon.Types
{
    public enum WeaponType
    {
        Sword
    }
}


public class Weapons : MonoBehaviour
{
    public static WeaponType weaponType = WeaponType.Sword;

    public virtual void Use()
    {
        Debug.Log("Generic Weapon used");
    }
}
