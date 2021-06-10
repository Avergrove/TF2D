using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rgbd;
    private Animator anim;

    public float jumpHeight;
    public float bhopTimeWindow;
    private float bhopTimeStamp;
    public Vector2 jumpBoost;

    public GameObject jumpFXObject;

    private Vector2 landingVelocity;
    private bool firstLandingFrame;

    void Start()
    {
        character = GetComponent<Character>();
        rgbd = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        bhopTimeStamp = 0;
        landingVelocity = Vector2.zero;
        firstLandingFrame = true;
    }

    private void FixedUpdate()
    {

        if (character.isGrounded && firstLandingFrame)
        {
            landingVelocity = rgbd.velocity;
            firstLandingFrame = false;
            bhopTimeStamp = bhopTimeWindow;
        }

        else if (!character.isGrounded)
        {
            firstLandingFrame = true;
        }

        if (bhopTimeStamp > 0)
        {
            bhopTimeStamp -= Time.deltaTime;
        }
    }

    public void Jump(Vector2 tiltValue)
    {
        if (bhopTimeStamp > 0 && tiltValue.x != 0)
        {
            BunnyHop(tiltValue);
        }

        else
        {
            rgbd.velocity = new Vector2(rgbd.velocity.x, jumpHeight);
            GameObject particleObject = GameObject.Instantiate(jumpFXObject);
            particleObject.transform.position = this.transform.position;
        }
    }


    private void BunnyHop(Vector2 tiltValue)
    {
        Vector2 boost = Vector2.zero;

        if (tiltValue.x > 0)
        {
            boost = jumpBoost;
        }

        else if (tiltValue.x < 0)
        {
            boost = -jumpBoost;
        }

        if (landingVelocity.x * rgbd.velocity.x > 0 && Mathf.Abs(landingVelocity.x) > Mathf.Abs(rgbd.velocity.x))
        {
            rgbd.velocity = new Vector2(landingVelocity.x, jumpHeight);
        }

        else
        {
            rgbd.velocity = new Vector2(rgbd.velocity.x, jumpHeight);
        }

        rgbd.AddForce(boost);
        bhopTimeStamp = 0;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position - new Vector2(0, 1.5f));
    }
}
