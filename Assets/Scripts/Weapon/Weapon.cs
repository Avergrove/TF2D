using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private GameObject owner;
    protected bool isEnabled;

    protected float cooldownTimeStamp;
    public float weaponCooldownTime;
    public float weaponReloadTime;

    public GameObject Owner { get => owner; set => owner = value; }

    protected virtual void Start()
    {
        // Method Stub
    }

    protected void Update()
    {
        if(cooldownTimeStamp != 0)
        {
            cooldownTimeStamp = Mathf.Clamp(cooldownTimeStamp - Time.deltaTime, 0, cooldownTimeStamp);
        }
    }

    /// <summary>
    /// Sets a weapon to be enabled or disabled, affecting whether they can be used or not.
    /// </summary>
    /// <param name="enable"></param>
    public void SetEnabled(bool enable)
    {
        isEnabled = enable;
    }

    public virtual void OnFirePressed(Vector2 direction) {
        cooldownTimeStamp = weaponCooldownTime;
    }

    public abstract void OnFireHeld();
    public abstract void OnFireReleased();
}
