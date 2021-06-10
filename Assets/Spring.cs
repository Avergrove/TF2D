using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// The Spring propels the player character in a specified direction.
/// </summary>
/// 

public class Spring : MonoBehaviour
{
    [Tooltip("The angle in degree to launch towards, anticlockwise with 0 degree being upwards.")]
    [Range(0, 360)]
    public float angle;

    [Tooltip("How powerful the spring will send the player towards the specified direction")]
    public float launchStrength;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Players")){
            Rigidbody2D playerRgbd = collision.GetComponent<Rigidbody2D>();
            playerRgbd.velocity = Vector2.zero;
            playerRgbd.AddForce(launchStrength * toDirectionVector(angle));
            audioSource.Play();
        }
    }

    private Vector2 toDirectionVector(float angle)
    {
        return Quaternion.Euler(0, 0, angle) * Vector2.up;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2) transform.position + toDirectionVector(angle) * launchStrength / 250);
    }
}
