using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Base class that defines behavior of a projectile.
 */
public abstract class Projectile : MonoBehaviour
{
    public GameObject shooter;

    public float velocity;
    public float gravityScale;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
