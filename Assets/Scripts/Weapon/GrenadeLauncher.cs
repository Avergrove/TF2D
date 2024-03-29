﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public GameObject projectile;

    protected override void Start()
    {
        base.Start();
    }

    public override void OnFirePressed(Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject generatedProjectile = GameObject.Instantiate(projectile);
        generatedProjectile.GetComponent<Projectile>().shooter = Owner;

        // Set transform
        generatedProjectile.transform.position = this.transform.position;
        generatedProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public override void OnFireHeld()
    {
        // TODO: Attempt to continue firing off cooldown
    }

    public override void OnFireReleased()
    {
        // nothing
    }
}
