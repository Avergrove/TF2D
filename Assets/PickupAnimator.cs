using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves a pickup collectible up and down.
/// </summary>
public class PickupAnimator : MonoBehaviour
{
    public float height;
    public float speed;
    
    Vector2 position;


    // Start is called before the first frame update
    void Start()
    {
        this.position = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float newY = Mathf.Sin(Time.time * speed) * height + this.position.y;
        transform.position = new Vector2(transform.position.x, newY);
    }
}
