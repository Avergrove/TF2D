using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

/**
 * Fires rockets, the weapon of choice of the Soldier.
 */
public class RocketLauncher : ProjectileWeapon
{

    protected override void Start()
    {
        base.Start();
    }

    /**
     * Fires a rocket in the pointed direction.
     */
    public override void OnFirePressed(Vector2 direction)
    {

        if (cooldownTimeStamp == 0)
        {
            float angle;
            if (!direction.Equals(Vector2.zero))
            {
                angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            }

            else
            {
                angle = transform.parent.eulerAngles.z;
            }

            GameObject generatedProjectile = GameObject.Instantiate(firedProjectile);
            generatedProjectile.GetComponent<Projectile>().shooter = Owner;

            // Set transform
            generatedProjectile.transform.position = this.transform.position;
            generatedProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

            // Set physics
            Rigidbody2D rgbd = generatedProjectile.GetComponent<Rigidbody2D>();
            Vector3 angles = generatedProjectile.transform.rotation.eulerAngles;

            base.OnFirePressed(direction);
        }
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
