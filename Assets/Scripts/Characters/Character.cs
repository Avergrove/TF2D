using UnityEngine;

public abstract class Character : MonoBehaviour, IControllable
{
    Rigidbody2D rgbd;
    GameObject weaponHolder;

    public int hp;
    public int currentHp;

    public Weapon equippedWeapon;

    public float shoulderDistance;

    protected float movementSpeed;
    public float movementSpeedMultiplier;

    protected float jumpHeight;
    public float jumpHeightMultiplier;
    public float maxJumpCount;

    public GameObject primaryWeaponPrefab;
    public GameObject secondaryWeaponPrefab;
    public GameObject meleeWeaponPrefab;

    // Start is called before the first frame update
    public virtual void Start()
    {
        this.rgbd = this.GetComponent<Rigidbody2D>();
        this.rgbd.gravityScale = 8;

        this.weaponHolder = this.transform.Find("WeaponHolder").gameObject;

        currentHp = hp;

        this.movementSpeed = 15;
        this.jumpHeight = 20;

        EquipWeapon(GameObject.Instantiate(primaryWeaponPrefab).GetComponent<Weapon>());
        EquipWeapon(GameObject.Instantiate(secondaryWeaponPrefab).GetComponent<Weapon>());
        EquipWeapon(GameObject.Instantiate(meleeWeaponPrefab).GetComponent<Weapon>());
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

    }

    public void OnFirePressed()
    {
        equippedWeapon.fire();
    }

    public virtual void OnJumpPressed()
    {
        rgbd.velocity = new Vector2(rgbd.velocity.x, jumpHeight * jumpHeightMultiplier);
    }

    public void OnHorizontalAxis(float value)
    {
        rgbd.velocity = new Vector2(movementSpeed * movementSpeedMultiplier * value, rgbd.velocity.y);
    }

    public void OnVerticalAxis(float value)
    {
        // No response needed... yet.
    }

    public void InflictDamage(int damage)
    {
        currentHp -= damage;
    }

    /**
     * Attaches a weapon to character, and automatically equip the character with it.
     * The gameObject must implement Weapon
     */
    public void EquipWeapon(Weapon weapon)
    {
        this.equippedWeapon = weapon;
        this.equippedWeapon.transform.SetParent(weaponHolder.transform);
        this.equippedWeapon.transform.localPosition = shoulderDistance * new Vector2(1, 0);
    }
}
