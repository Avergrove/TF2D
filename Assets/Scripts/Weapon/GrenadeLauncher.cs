using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeLauncher : Weapon
{
    public GameObject projectile;

    protected override void Start()
    {
        base.Start();
    }

    /**
     * Fires a rocket in the pointed direction.
     */
    public override void OnFirePressed()
    {
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorInWorldPos - (Vector2)transform.parent.position;
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
