using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The flamethrower spits a cone of fire in front of it, burning enemies in its path proportional to range.
/// <br></br>
/// TODO: Airblast - The flamethrower is also able to reflect projectiles to a selected trajectory. The flamethrower is unable to spit fire during the process.
/// </summary>
public class Flamethrower : Weapon
{
    ParticleSystem ps;

    protected override void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var emission = ps.emission;
        emission.enabled = false;
    }

    public override void OnFirePressed(Vector2 direction)
    {

        // Turns on the flamethrower
        var emission = ps.emission;
        emission.enabled = true;

    }

    public override void OnFireHeld()
    {

    }

    public override void OnFireReleased()
    {
        var emission = ps.emission;
        emission.enabled = false;
    }


}
