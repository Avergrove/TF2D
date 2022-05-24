using System;
using UnityEngine;
using static Direction;

/// <summary>
/// Defines the stats and state of a character
/// TODO: Separate movement, this is supposed to be about stats, not movement!
/// </summary>
public class Character : MonoBehaviour
{
    public Rigidbody2D rgbd;
    GameObject weaponHolder;
    AudioSource aSource;
    CharacterMovement characterMovement;
    SpriteRenderer sr;

    public int hp;
    public int currentHp;

    public Weapon equippedWeapon;

    public float shoulderDistance;

    public DirectionEnum facingDirection;

    public float jumpHeight;
    public float jumpBoost;
    public float maxJumpCount;

    public GameObject defaultPrimaryWeapon;
    public GameObject defaultSecondaryWeapon;
    public GameObject defaultMeleeWeapon;

    private GameObject[] weapons;

    public AudioClip jumpSound;
    public GameObject jumpParticleObject;

    // Movement State
    public bool isGrounded;

    public bool isAttachedToMovingPlatform;
    Rigidbody2D attachedRgbd;

    public bool isClutched;

    public virtual void Start()
    {
        this.rgbd = this.GetComponent<Rigidbody2D>();
        this.aSource = this.GetComponent<AudioSource>();
        this.characterMovement = this.GetComponent<CharacterMovement>();
        this.sr = this.GetComponent<SpriteRenderer>();


        this.weaponHolder = this.transform.Find("WeaponHolder").gameObject;

        currentHp = hp;

        this.facingDirection = DirectionEnum.Right;

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
        /*
        anim.SetBool("isGrounded", isGrounded);
        anim.SetFloat("speedX", Math.Abs(this.rgbd.velocity.x));
        anim.SetFloat("velocityY", this.rgbd.velocity.y);
        anim.SetFloat("speedY", Math.Abs(this.rgbd.velocity.y));
        */
        if (facingDirection.Equals(DirectionEnum.Right)){
            sr.flipX = false;
        }

        else if(facingDirection.Equals(DirectionEnum.Left))
        {
            sr.flipX = true;
        }
    }

    public void Jump(Vector2 tiltValue)
    {
        if (isGrounded)
        {
            characterMovement.Jump(tiltValue);
            aSource.PlayOneShot(jumpSound, 0.15f);

            // Create jump particle
            GameObject createdJumpParticle = GameObject.Instantiate(jumpParticleObject);
            createdJumpParticle.transform.position = this.transform.position;
        }
    }

    public void Move(Vector2 tiltValue)
    {
        characterMovement.Move(tiltValue);
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

    public void OnClutchDown()
    {
        isClutched = true;
    }

    public void OnClutchReleased()
    {
        isClutched = false;
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

    public void PlaySound()
    {
        aSource.PlayOneShot(jumpSound);
    }
}
