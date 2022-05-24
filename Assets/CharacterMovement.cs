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

    public float maxGroundSpeed;
    public float groundAcceleration; // Described as a percentage of max speed, 0.05f for example will accelerate the character by 5% per frame.
    public float groundSpeedDecay;   // Described a percentage, the character will lose 0.05% speed per frame, for example.
    public float groundOverspeedDecay; // Describes the percentage speed lost when the button is held down in the same direction when over maximum speed.

    public float airAcceleration;
    public float airSpeedDecay;
    public float maxMovementSpeed;

    protected float prevClutchedMaxSpeed;

    protected float maxUnclutchedAirSpeed;
    public float maxAirControlSpeedMultiplier;

    public float jumpHeight;
    public Vector2 jumpBoost;

    public GameObject jumpFXObject;

    private Vector2 landingVelocity;
    private bool firstLandingFrame;

    void Start()
    {
        character = GetComponent<Character>();
        rgbd = GetComponent<Rigidbody2D>();

        landingVelocity = Vector2.zero;
        firstLandingFrame = true;
    }

    private void FixedUpdate()
    {

        if (character.isGrounded && firstLandingFrame)
        {
            landingVelocity = rgbd.velocity;
            firstLandingFrame = false;
        }

        else if (!character.isGrounded)
        {
            firstLandingFrame = true;
            if(Math.Abs(rgbd.velocity.x) > Math.Abs(prevClutchedMaxSpeed))
            {
                prevClutchedMaxSpeed = Math.Abs(rgbd.velocity.x);
            }
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
            // If moving in same direction
            if (tiltValue.x * rgbd.velocity.x > 0)
            {
                // If lower than max speed, then slowly accelerate up to it
                if (Math.Abs(rgbd.velocity.x) < Math.Abs(maxGroundSpeed))
                {
                    rgbd.velocity = new Vector2(Mathf.Lerp(Math.Abs(rgbd.velocity.x), maxGroundSpeed, groundAcceleration) * Mathf.Sign(tiltValue.x), 0);
                }

                // If faster than max speed, then slow down the decay to max velocity
                else if(Math.Abs(rgbd.velocity.x) > Math.Abs(maxGroundSpeed))
                {
                    rgbd.velocity = new Vector2(Mathf.Lerp(Math.Abs(rgbd.velocity.x), maxGroundSpeed, 100 - groundOverspeedDecay) * Mathf.Sign(tiltValue.x), 0);
                }

                // If equal to max speed, keep max speed.
                else
                {
                    rgbd.velocity = new Vector2(maxGroundSpeed * Mathf.Sign(rgbd.velocity.x), rgbd.velocity.y);
                }
            }

            // If moving in different direction
            else if(tiltValue.x * rgbd.velocity.x < 0)
            {
                rgbd.velocity = new Vector2(Mathf.Lerp(Math.Abs(rgbd.velocity.x), -maxGroundSpeed, groundAcceleration) * Mathf.Sign(rgbd.velocity.x), rgbd.velocity.y);
            }

            // If the player wishes to move from a standstill.
            else if(tiltValue.x != 0)
            {
                rgbd.velocity = new Vector2(Mathf.Lerp(Math.Abs(rgbd.velocity.x), maxGroundSpeed, groundAcceleration) * Mathf.Sign(tiltValue.x), rgbd.velocity.y);
            }

            // Decay to 0 if there is no input
            else
            {
                // If decaying from right
                if(rgbd.velocity.x > 0)
                {
                    rgbd.velocity = new Vector2(Mathf.Lerp(Math.Abs(rgbd.velocity.x), 0, groundSpeedDecay) * Mathf.Sign(rgbd.velocity.x), rgbd.velocity.y);
                }

                // If decaying from left
                else
                {
                    rgbd.velocity = new Vector2(Mathf.Lerp(0, Math.Abs(rgbd.velocity.x), 100f - groundSpeedDecay) * Mathf.Sign(rgbd.velocity.x), rgbd.velocity.y);
                }

                // 0 case is not needed, already at standstill.
            }
        }

        // Air Movement
        else
        {
            float multResult = tiltValue.x * rgbd.velocity.x;

            // Activating clutch will mimic the strafing behavior of source-based games
            if (character.isClutched)
            {
                // Character continues moving in same direction
                if (tiltValue.x == 0)
                {
                    // Nothing happens if no joystick input is given, character will move at same speed forward.
                }

                // Characters moving in the same direction can move forward very slightly if at a low velocity.
                else if(tiltValue.x * rgbd.velocity.x >= 0)
                {
                    rgbd.AddForce(Vector2.zero);
                }

                // Character moving in reverse direction can turn in the other direction up to max speed attained before landing.
                else if(tiltValue.x * rgbd.velocity.x < 0)
                {
                    if (Math.Abs(rgbd.velocity.x) < Math.Abs(prevClutchedMaxSpeed))
                    {
                        rgbd.AddForce(Vector2.zero);
                    }
                }


            }

            // An unclutched character will behave like a regular platformer character, slowing down when no input is added, while backwards movement is slower.
            else
            {
                // When same direction of input, increase up to max air velocity.
                if (multResult > 0)
                {
                    if (Math.Abs(rgbd.velocity.x) < maxUnclutchedAirSpeed)
                    {
                        rgbd.AddForce(new Vector2(Direction.ConsiderDirection(character.facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
                        if (rgbd.velocity.x > maxUnclutchedAirSpeed) rgbd.velocity = new Vector2(maxUnclutchedAirSpeed, rgbd.velocity.y);
                    }
                }

                // When different direction of input, increase up to opposite direction max velocity.
                else if (multResult < 0)
                {
                    rgbd.AddForce(new Vector2(Direction.ConsiderDirection(character.facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
                }

                // Decay to 0 speed if no input is given.
                else
                {
                    rgbd.velocity = new Vector2(rgbd.velocity.x * airSpeedDecay, rgbd.velocity.y);
                }
            }
        }
    }

    public void Jump(Vector2 tiltValue)
    {
            rgbd.velocity = new Vector2(rgbd.velocity.x, jumpHeight);
            GameObject particleObject = GameObject.Instantiate(jumpFXObject);
            particleObject.transform.position = this.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, (Vector2)transform.position - new Vector2(0, 1.5f));
    }

    
}
