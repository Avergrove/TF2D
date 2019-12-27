using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    private GameObject owner;
    protected bool isEnabled;

    public GameObject Owner { get => owner; set => owner = value; }

    protected virtual void Start()
    {
        // Method Stub
    }

    protected void Update()
    {
        // Method Stub
    }

    /// <summary>
    /// Sets a weapon to be enabled or disabled, affecting whether they can be used or not.
    /// </summary>
    /// <param name="enable"></param>
    public void setEnabled(bool enable)
    {
        isEnabled = enable;
    }

    public abstract void OnFirePressed();
    public abstract void OnFireHeld();
    public abstract void OnFireReleased();
}
