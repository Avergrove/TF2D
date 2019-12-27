using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * The flamethrower fires particles that goes through enemies and deals damage when held down
 */
public class Flamethrower : Weapon
{
    public override void OnFirePressed()
    {
        // Turns on the flamethrower
    }

    public override void OnFireHeld()
    {
        // nothing
    }

    public override void OnFireReleased()
    {
        // Turns off the flamethrower
    }


}
