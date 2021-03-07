using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Weapon", fileName = "New Weapon")]
[System.Serializable]
public class Weapon : ScriptableObject
{
    public enum WeaponType { Sword, Knife, Dagger, Axe, Hammer, Bow, Staff };
    public WeaponType weaponType;
    public bool isTwoHanded = false;
    public float weaponDamage;
    public float weaponDurability;
    public float weaponKnockbackRate;
    public float weaponTimeBetweenAttacks;
    public AnimationClip weaponAnimation;

    public Vector2 weaponColliderSize;
    public Vector2 weaponColliderOffset;

    public AnimationClip GetWeaponAnimation()
    {
        return weaponAnimation;
    }
}
