using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A grenade launcher pill rolls along the floor when it hits an object, exploding when the fuse times out.
/// </summary>
public class GrenadeLauncherPill : Projectile
{
    Collider2D coll2d;

    public float initFuseTime;
    public int contactDamage;
    public float knockback;
    public float explosionRadius;
    public GameObject explosionParticles;

    private float remainingFuseTime;
    private bool hasTouchedPlatform;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        coll2d = this.GetComponent<Collider2D>();

        if (shooter != null)
        {
            Physics2D.IgnoreCollision(coll2d, shooter.GetComponent<Collider2D>());
        }

        remainingFuseTime = initFuseTime;
        hasTouchedPlatform = false;
    }

    /// <summary>
    /// Update is used to keep track of the fuse time on the grenade.
    /// </summary>
    protected override void Update()
    {
        base.Update();

        remainingFuseTime -= Time.deltaTime;
        if(remainingFuseTime <= 0)
        {
            Collider2D[] affectedTargets = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);
            Detonate();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        // A grenade will not explode directly on players if it has touched a platform.
        if (!hasTouchedPlatform)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Characters"))
            {
                DirectlyDamage(collision.gameObject);
                Detonate();
            }

            else if (collision.gameObject.layer == LayerMask.NameToLayer("Platforms"))
            {
                hasTouchedPlatform = true;
            }
        }
    }

    void DirectlyDamage(GameObject collidedObject)
    {
        // collidedObject.dealDamage();
    }

    void Detonate()
    {
        // Detonate on nearby targets.
        Collider2D[] affectedTargets = Physics2D.OverlapCircleAll(this.transform.position, explosionRadius);

        // Apply knockback to enemies and self in the vicinity
        // The angle is the angle between floor, and explosion to affectedTarget center of mass
        foreach (Collider2D affectedTarget in affectedTargets)
        {

            float dist = Vector3.Distance(affectedTarget.transform.position, this.transform.position);
            Vector3 dir = (affectedTarget.transform.position - this.transform.position).normalized;

            // TODO: Add damage and knockback scaling to distance.
            if (affectedTarget.CompareTag("Players"))
            {
                affectedTarget.GetComponent<Rigidbody2D>().AddForce(dir * knockback);
            }
        }

        // DIE
        GameObject generatedParticles = GameObject.Instantiate(explosionParticles);
        generatedParticles.transform.position = this.transform.position;

        Destroy(generatedParticles.gameObject, 2f);
        Destroy(this.gameObject);
    }
}
