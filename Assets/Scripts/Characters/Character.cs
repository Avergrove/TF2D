using System;
using System.Collections.Generic;
using UnityEditor.UIElements;
using UnityEngine;
using static Direction;

/// <summary>
/// Defines the stats of a character
/// </summary>
public class Character : MonoBehaviour
{
    Rigidbody2D rgbd;
    GameObject weaponHolder;
    AudioSource aSource;

    public int hp;
    public int currentHp;

    public Weapon equippedWeapon;

    public float shoulderDistance;

    private DirectionEnum facingDirection;

    public float accelerationForce;
    public float airAcceleration;
    public float airDeceleration;
    public float maxMovementSpeed;

    protected float maxAirControlSpeed;
    public float maxAirControlSpeedMultiplier;

    public float jumpHeight;
    public float jumpBoost;
    public float maxJumpCount;

    public GameObject defaultPrimaryWeapon;
    public GameObject defaultSecondaryWeapon;
    public GameObject defaultMeleeWeapon;

    private GameObject[] weapons;

    public AudioClip jumpSound;
    public GameObject jumpParticleObject;

    public bool isGrounded;
    private bool firstGroundedFrame;
    private GameObject groundedObject;
    private Vector2 lastAttachedRgbdPosition;

    public bool isAttachedToMovingPlatform;
    Rigidbody2D attachedRgbd;

    public virtual void Start()
    {
        this.rgbd = this.GetComponent<Rigidbody2D>();
        this.aSource = this.GetComponent<AudioSource>();

        this.weaponHolder = this.transform.Find("WeaponHolder").gameObject;

        currentHp = hp;

        this.facingDirection = DirectionEnum.Right;

        this.maxAirControlSpeed = 3f;

        weapons = new GameObject[3];

        this.GiveWeapon(defaultPrimaryWeapon, 0);
        this.GiveWeapon(defaultSecondaryWeapon, 1);
        this.GiveWeapon(defaultMeleeWeapon, 2);

        this.EquipWeapon(0);

        this.lastAttachedRgbdPosition = Vector2.positiveInfinity;
    }

    // Update is called once per frame
    public virtual void FixedUpdate()
    {
        isGrounded = GroundCheck();
    }

    public void Move(Vector2 tiltValue)
    {

        if (tiltValue.x > 0)
        {
            facingDirection = DirectionEnum.Right;
        }

        else if (tiltValue.x < 0)
        {
            facingDirection = DirectionEnum.Left;
        }

        if (isGrounded)
        {
            Vector2 parentVelocity = Vector2.zero;
            if (isAttachedToMovingPlatform)
            {
                parentVelocity = attachedRgbd.velocity;
            }

            float localMaxMov = maxMovementSpeed + parentVelocity.x;
            float localMinMov = -maxMovementSpeed + parentVelocity.x;

            if(tiltValue.x == 0)
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

        else
        {
            float multResult = tiltValue.x * rgbd.velocity.x;

            // No x input, decay to 0 speed.
            if (tiltValue.x == 0)
            {
                if (rgbd.velocity.x > 0)
                {
                    rgbd.AddForce(new Vector2(-airDeceleration, 0));
                    if (rgbd.velocity.x < 0) rgbd.velocity = new Vector2(0, rgbd.velocity.y);
                }

                else if(rgbd.velocity.x < 0)
                {
                    rgbd.AddForce(new Vector2(airDeceleration, 0));
                    if (rgbd.velocity.x > 0) rgbd.velocity = new Vector2(0, rgbd.velocity.y);
                }
            }

            // Same direction
            else if (multResult >= 0)
            {
                if (Math.Abs(rgbd.velocity.x) < maxAirControlSpeed)
                {
                    rgbd.AddForce(new Vector2(Direction.ConsiderDirection(facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
                    if (rgbd.velocity.x > maxAirControlSpeed) rgbd.velocity = new Vector2(maxAirControlSpeed, rgbd.velocity.y);
                }
            }

            // Reverse direction
            else if (multResult < 0)
            {
                rgbd.AddForce(new Vector2(Direction.ConsiderDirection(facingDirection, airAcceleration) * Math.Abs(tiltValue.x), 0));
            }
        }
    }

    public void PointWithMouse()
    {
        // Point weaponholder towards cursor, if available.
        Vector2 mousePos = Input.mousePosition;
        Vector2 objectPos = Camera.main.WorldToScreenPoint(weaponHolder.transform.position);
        Vector2 diff = new Vector2(mousePos.x - objectPos.x, mousePos.y - objectPos.y);

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        weaponHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    public void PointWithJoystick(Vector2 tilt)
    {
        // TODO: Fix XY axis snapping
        if (!tilt.Equals(Vector2.zero))
        {
            float angle = Mathf.Atan2(tilt.y, tilt.x) * Mathf.Rad2Deg;
            weaponHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
    }

    private bool GroundCheck()
    {
        int layerMask = (1 << LayerMask.NameToLayer("Platforms"));
        Collider2D groundCollider = Physics2D.OverlapArea(new Vector2(transform.position.x - 0.49f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.49f, transform.position.y - 1.5f), layerMask);
        return groundCollider;

    }

    public void OnVerticalAxis(float value)
    {
        // No response needed... yet.
    }

    public void InflictDamage(int damage)
    {
        currentHp -= damage;
    }

    /// <summary>
    /// Adds a weapon to the player's inventory
    /// </summary>
    /// <param name="weaponObject">The weapon to add in</param>
    /// <param name="slot">Which slot to add it to, starting from 0 for primary weapon.</param>
    public void GiveWeapon(GameObject weaponObject, int slot)
    {
        if (weaponObject != null)
        {
            GameObject newObject = GameObject.Instantiate(weaponObject);
            newObject.transform.SetParent(weaponHolder.transform);
            newObject.transform.localPosition = shoulderDistance * new Vector2(1, 0);
            newObject.GetComponent<Weapon>().Owner = this.gameObject;
            newObject.GetComponent<SpriteRenderer>().enabled = false;

            this.weapons[slot] = newObject;
        }
    }

    /// <summary>
    /// Equips the character with weapon on slot slot
    /// </summary>
    /// <param name="slot">The slot for the weapon to pick from, starting from 0 for primary weapon.</param>
    public void EquipWeapon(int slot)
    {
        this.equippedWeapon = weapons[slot].GetComponent<Weapon>();
        this.equippedWeapon.GetComponent<SpriteRenderer>().enabled = true;
    }

    /// <summary>
    /// Stows a weapon away for later use.
    /// </summary>
    public void StowWeapon()
    {
        if (this.equippedWeapon != null)
        {
            this.equippedWeapon.GetComponent<SpriteRenderer>().enabled = false;
            this.equippedWeapon = null;
        }
    }

    public void Fire(Vector2 direction)
    {
        equippedWeapon.OnFirePressed(direction);
    }

    public void OnFireHeld()
    {
        equippedWeapon.OnFireHeld();
    }

    public void OnFireReleased()
    {
        equippedWeapon.OnFireReleased();
    }


    public void Jump(Vector2 tiltValue)
    {
        // TODO: Boost only if analog stick is being pushed.
        if (isGrounded)
        {
            float boost = tiltValue.x != 0 ? Direction.ConsiderDirection(facingDirection, jumpBoost) : 0;
            rgbd.velocity = new Vector2(rgbd.velocity.x + boost, jumpHeight);
            aSource.PlayOneShot(jumpSound, 0.1f);

            // Create jump particle
            GameObject createdJumpParticle = GameObject.Instantiate(jumpParticleObject);
            createdJumpParticle.transform.position = this.transform.position;
        }
    }

    public void InformAttachmentToMovingPlatform(Rigidbody2D attachedRgbd, bool isAttached)
    {
        if (isAttached)
        {
            this.attachedRgbd = attachedRgbd;
            isAttachedToMovingPlatform = true;
        }

        else
        {
            this.attachedRgbd = null;
            isAttachedToMovingPlatform = false;
        }
    }

    /// <summary>
    /// 	A Bunny Hop occurs when tapping jump in a short frame of time during landing. A successful bunny hop will grant a small burst of speed.
    /// </summary>
    public virtual void BunnyHop()
    {

    }

    public void OnSlot1()
    {
        this.StowWeapon();
        this.EquipWeapon(0);
    }

    public void OnSlot2()
    {
        this.StowWeapon();
        this.EquipWeapon(1);
    }

    public void OnSlot3()
    {
        this.StowWeapon();
        this.EquipWeapon(2);
    }
}
