using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// A force field radiates force on a player.
/// </summary>
public class ForceField : MonoBehaviour
{
    public float strength;
    private float radius;
    private List<Rigidbody2D> affRgbds;


    // Start is called before the first frame update
    void Start()
    {
        affRgbds = new List<Rigidbody2D>();
        radius = this.GetComponent<CircleCollider2D>().radius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Players"))
        {
            affRgbds.Add(collision.gameObject.GetComponent<Rigidbody2D>());
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Players"))
        {
            foreach (Rigidbody2D rgbd in affRgbds)
            {
                if (rgbd != null)
                {
                    Vector2 displacement = rgbd.transform.position - this.transform.position;
                    float magnitude = displacement.magnitude;
                    Vector2 direction = displacement.normalized;
                    float adjustedStrength = Mathf.Lerp(0, strength, magnitude / radius);
                    
                    rgbd.AddForce(direction * adjustedStrength);
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Players"))
        {
            affRgbds.Remove(collision.gameObject.GetComponent<Rigidbody2D>());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawSphere(transform.position, radius);
    }
}
