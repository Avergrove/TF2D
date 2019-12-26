using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Fires rockets, the weapon of choice of Soldier.
 */
public class RocketLauncher : Weapon
{
    public GameObject projectile;

    protected override void Start()
    {
        base.Start();
    }

    /**
     * Fires a rocket in the pointed direction.
     */
    public override void fire()
    {
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorInWorldPos - (Vector2)transform.parent.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        GameObject generatedProjectile = GameObject.Instantiate(projectile);
        
        // Set transform
        generatedProjectile.transform.position = this.transform.position;
        generatedProjectile.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Set physics
        Rigidbody2D rgbd = generatedProjectile.GetComponent<Rigidbody2D>();
        Vector3 angles = generatedProjectile.transform.rotation.eulerAngles;
        float velocity = generatedProjectile.GetComponent<Rocket>().velocity;
        rgbd.velocity = new Vector2(velocity * Mathf.Cos(angles.z * Mathf.Deg2Rad), velocity * Mathf.Sin(angles.z * Mathf.Deg2Rad));
    }
}
