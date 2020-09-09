using System;
using System.Collections.Generic;
using UnityEngine;
using static Direction;

/// <summary>
/// Defines the stats of a character
/// </summary>
public abstract class Character : MonoBehaviour
{
    Rigidbody2D rgbd;
    GameObject weaponHolder;

    public int hp;
    public int currentHp;

    public Weapon equippedWeapon;

    public float shoulderDistance;

    private DirectionEnum facingDirection;

    public float acceleration;
    public float maxMovementSpeed;

    protected float maxAirControlSpeed;
    public float maxAirControlSpeedMultiplier;

    protected float jumpHeight;
    public float jumpBoost;
    public float maxJumpCount;

    public GameObject defaultPrimaryWeapon;
    public GameObject defaultSecondaryWeapon;
    public GameObject defaultMeleeWeapon;

    private GameObject[] weapons;

    public bool isGrounded;

    public virtual void Start()
    {
        this.rgbd = this.GetComponent<Rigidbody2D>();
        this.rgbd.gravityScale = 5.5f;

        this.weaponHolder = this.transform.Find("WeaponHolder").gameObject;

        currentHp = hp;

        this.facingDirection = DirectionEnum.Right;

        this.maxAirControlSpeed = 3f;
        this.jumpHeight = 25;

        weapons = new GameObject[3];

        this.GiveWeapon(defaultPrimaryWeapon, 0);
        this.GiveWeapon(defaultSecondaryWeapon, 1);
        this.GiveWeapon(defaultMeleeWeapon, 2);

        this.EquipWeapon(0);
    }

    // Update is called once per frame
    public virtual void Update()
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
            if (Math.Abs(rgbd.velocity.x) < maxMovementSpeed)
            {
                float resultingXVelocity = rgbd.velocity.x + Direction.ConsiderDirection(facingDirection, acceleration) * Math.Abs(tiltValue.x);

                rgbd.velocity = new Vector2(Mathf.Clamp(resultingXVelocity, -maxMovementSpeed, maxMovementSpeed), rgbd.velocity.y);
            }
        }

        // TODO: 2D Character strafing, maximum speed is saved until you land on the ground
        else
        {
            // TODO: Fix case when you have higher velocity
            if (Math.Abs(rgbd.velocity.x) < maxAirControlSpeed)
            {
                // TODO: Decrease acceleration as character approaches max speed: Check Lerp
                rgbd.velocity = new Vector2(rgbd.velocity.x + Direction.ConsiderDirection(facingDirection, acceleration) * Math.Abs(tiltValue.x), rgbd.velocity.y);
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
        return Physics2D.OverlapArea(new Vector2(transform.position.x - 0.5f, transform.position.y - 0.5f), new Vector2(transform.position.x + 0.5f, transform.position.y - 1.5f), layerMask);
 
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

        GameObject newObject = GameObject.Instantiate(weaponObject);
        newObject.transform.SetParent(weaponHolder.transform);
        newObject.transform.localPosition = shoulderDistance * new Vector2(1, 0);
        newObject.GetComponent<Weapon>().Owner = this.gameObject;
        newObject.GetComponent<SpriteRenderer>().enabled = false;

        this.weapons[slot] = newObject;

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

    
    public virtual void Jump()
    {
        // TODO: Boost only if analog stick is being pushed.
        if (isGrounded)
        {
            float boost = Direction.ConsiderDirection(facingDirection, jumpBoost);
            rgbd.velocity = new Vector2(rgbd.velocity.x + boost, jumpHeight);
        }
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
