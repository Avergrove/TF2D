using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base class that defines behavior of a projectile.
 */
public abstract class Projectile : MonoBehaviour
{
    public GameObject shooter;
    protected Rigidbody2D rgbd;


    public float entityLifetime; // The amount of time before the projectile destroys itself.
    public float travelSpeed; // The initial travel speed of the projectile
    public float gravityScale; // How much gravity is the projectile affected by

    // Start is called before the first frame update
    protected virtual void Start()
    {
        rgbd = this.GetComponent<Rigidbody2D>();
        if(rgbd == null) throw new MissingComponentException("Projectile scripts should come with Rigidbody2D");

        rgbd.velocity = TravelSpeedToLocalRotationVector(travelSpeed);
        rgbd.gravityScale = this.gravityScale;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    /// <summary>
    /// Creates a Vector2 in the projectile's facing direction using preferred travel speed.
    /// </summary>
    /// <returns></returns>
    protected Vector2 TravelSpeedToLocalRotationVector(float travelSpeed)
    {
        // Set physics
        Vector3 angles = this.transform.rotation.eulerAngles;
        return new Vector2(travelSpeed * Mathf.Cos(angles.z * Mathf.Deg2Rad), travelSpeed * Mathf.Sin(angles.z * Mathf.Deg2Rad));
    }
}
