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
    
    Vector2 initialPosition;


    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        float newY = Mathf.Sin(Time.time * speed) * height + this.initialPosition.y;
        transform.localPosition = new Vector2(transform.localPosition.x, newY);
    }
}
