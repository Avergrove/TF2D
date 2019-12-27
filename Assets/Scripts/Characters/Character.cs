using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour, IControllable
{
    Rigidbody2D rgbd;
    Collider2D colliderComp;
    GameObject weaponHolder;

    public int hp;
    public int currentHp;

    public Weapon equippedWeapon;

    public float shoulderDistance;

    protected float movementSpeed;
    public float movementSpeedMultiplier;

    protected float maxAirControlSpeed;
    public float maxAirControlSpeedMultiplier;

    protected float jumpHeight;
    public float jumpHeightMultiplier;
    public float maxJumpCount;

    public GameObject defaultPrimaryWeapon;
    public GameObject defaultSecondaryWeapon;
    public GameObject defaultMeleeWeapon;

    private GameObject[] weapons;

    public float groundCheckDistance = 1.5f;
    private bool isGrounded;

    // Debug
    public Collider2D debugCollider;

    // Start is called before the first frame update
    public virtual void Start()
    {

        this.colliderComp = this.GetComponent<Collider2D>();
        this.rgbd = this.GetComponent<Rigidbody2D>();
        this.rgbd.gravityScale = 5.5f;

        this.weaponHolder = this.transform.Find("WeaponHolder").gameObject;

        currentHp = hp;

        this.movementSpeed = 13.5f;
        this.maxAirControlSpeed = 3f;
        this.jumpHeight = 20;

        weapons = new GameObject[3];

        this.GiveWeapon(defaultPrimaryWeapon, 0);
        this.GiveWeapon(defaultSecondaryWeapon, 1);
        this.GiveWeapon(defaultMeleeWeapon, 2);

        this.EquipWeapon(0);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        // Point weaponholder towards cursor, if available.
        Vector2 mousePos = Input.mousePosition;
        Vector2 objectPos = Camera.main.WorldToScreenPoint(weaponHolder.transform.position);
        Vector2 diff = new Vector2(mousePos.x - objectPos.x, mousePos.y - objectPos.y);

        float angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        weaponHolder.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        // Ground check, applys overlapSquare to two ends of the model to check whether character is grounded.
        Vector2 modelSize = colliderComp.bounds.size;

        // TODO: Replace raycast with overlapArea for ground detection.
        RaycastHit2D raycastHit = Physics2D.Raycast(transform.position, Vector2.down, modelSize.x / 2 + groundCheckDistance, 1 << 8);
        isGrounded = raycastHit;
    }

    public void OnHorizontalAxis(float tiltValue)
    {
        if (isGrounded)
        {
            rgbd.velocity = new Vector2(movementSpeed * movementSpeedMultiplier * tiltValue, rgbd.velocity.y);
        }

        // Speed should slowly change, instead of instant change.
        // If pressing the reverse direction, character should stop.
        // if horizontalVelocity = 0, accelerate to maxAirControlSpeed slowly.
        else
        {
            if(Mathf.Abs(rgbd.velocity.x) > maxAirControlSpeed)
            {
                // Airbrake
                if(Mathf.Sign(rgbd.velocity.x) != Mathf.Sign(tiltValue))
                {
                    rgbd.velocity = new Vector2(maxAirControlSpeed * tiltValue, rgbd.velocity.y);
                }
            }

            // Allow wiggle room in the air.
            else if(rgbd.velocity.x <= maxAirControlSpeed)
            {
                rgbd.velocity = new Vector2(maxAirControlSpeed * tiltValue, rgbd.velocity.y);
            }
        }
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

    public void OnFirePressed()
    {
        equippedWeapon.OnFirePressed();
    }

    public void OnFireHeld()
    {
        equippedWeapon.OnFireHeld();
    }

    public void OnFireReleased()
    {
        equippedWeapon.OnFireReleased();
    }

    public virtual void OnJumpPressed()
    {
        if (isGrounded)
        {
            rgbd.velocity = new Vector2(rgbd.velocity.x, jumpHeight * jumpHeightMultiplier);
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
