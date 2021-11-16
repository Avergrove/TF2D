using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

[RequireComponent(typeof(Character))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterMovement : MonoBehaviour
{
    private Character character;
    private Rigidbody2D rgbd;

    public float accelerationForce;
    public float airAcceleration;
    public float airDeceleration;
    public float maxMovementSpeed;

    protected float prevClutchedMaxSpeed;

    protected float maxUnclutchedAirSpeed;
    public float maxAirControlSpeedMultiplier;

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

    public void Move(Vector2 tiltValue)
    {
        if (tiltValue.x > 0)
        {
            character.facingDirection = DirectionEnum.Right;
        }

        else if (tiltValue.x < 0)
        {
            character.facingDirection = DirectionEnum.Left;
        }

        if (character.isGrounded)
        {
            Vector2 parentVelocity = Vector2.zero;
            if (character.isAttachedToMovingPlatform)
            {
                parentVelocity = rgbd.velocity;
            }

            float localMaxMov = maxMovementSpeed + parentVelocity.x;
            float localMinMov = -maxMovementSpeed + parentVelocity.x;

            if (tiltValue.x == 0)
            {
                // Do nothing, let x decay to 0 speed.
            }

            // If moving in same direction
            else if (tiltValue.x * rgbd.velocity.x > 0)
            {
                if (localMinMov <= rgbd.velocity.x && rgbd.velocity.x <= localMaxMov)
                {
                    rgbd.AddForce(new Vector2(accelerationForce * tiltValue.x, 0));
                }
            }

            else
            {
                rgbd.AddForce(new Vector2(accelerationForce * tiltValue.x, 0));
            }
        }

        // Air Movement
        else
        {
            float multResult = tiltValue.x * rgbd.velocity.x;

            // A clutched character has strafe controls on, and will not lose speed even when not holding keys, mimicking the strafe behaviour in Source games
            if (character.isClutched)
            {
                // Character continues moving in same direction
                if(tiltValue.x == 0)
                {
                    // Continue accelerating to last reverse's max speed.

                }

                // Character moving in reverse direction will gradually turn in the other direction in the same max speed. Max speed is reset after landing.
                else
                {
                    
                }

            }

            // An unclutched character will behave like a regular platformer character, slowing down when no input is added, while backwards movement is slower.
            else
            {
                if (tiltValue.x == 0)
                {
                    float prevX = rgbd.velocity.x;
                    if (rgbd.velocity.x > 0)
                    {
                        rgbd.velocity = new Vector2(Mathf.Clamp(rgbd.velocity.x - airDeceleration, 0, prevX), rgbd.velocity.y);
                    }

                    else if (rgbd.velocity.x < 0)
                    {
                        rgbd.velocity = new Vector2(Mathf.Clamp(rgbd.velocity.x + airDeceleration, prevX, 0), rgbd.velocity.y);
                    }
                }

                // Same direction
                else if (multResult >= 0)
                {
                    if (Math.Abs(rgbd.velocity.x) < maxUnclutchedAirSpeed)
                    {
                        rgbd.AddForce(new Vector2(Direction.ConsiderDirection(character.facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
                        if (rgbd.velocity.x > maxUnclutchedAirSpeed) rgbd.velocity = new Vector2(maxUnclutchedAirSpeed, rgbd.velocity.y);
                    }
                }

                // Reverse direction
                else if (multResult < 0)
                {
                    rgbd.AddForce(new Vector2(Direction.ConsiderDirection(character.facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
                }
            }
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
