using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A weapon that fires projectiles, for example, rocket launcher and grenade launcher.
/// 
/// Programatically, it spawns a projectile.
/// </summary>
public abstract class ProjectileWeapon : Weapon
{
    public GameObject firedProjectile;
}