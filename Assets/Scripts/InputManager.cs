using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a player's controllers, can be hooked up the controllable objects.
 */
public class InputManager : MonoBehaviour
{

    public GameObject controllingObject;
    private IControllable controllable;

    // Start is called before the first frame update
    void Start()
    {
        controllable = controllingObject.GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {
        controllable.OnHorizontalAxis(Input.GetAxis("Horizontal"));
        controllable.OnVerticalAxis(Input.GetAxis("Vertical"));

        if (Input.GetButtonDown("Jump"))
        {
            controllable.OnJumpPressed();
        }

        // Fire handling
        if (Input.GetButtonDown("Fire1"))
        {
            controllable.OnFirePressed();
        }

        else if (Input.GetButton("Fire1"))
        {
            controllable.OnFireHeld();
        }

        else if (Input.GetButtonUp("Fire1"))
        {
            controllable.OnFireReleased();
        }

        // Weapon slots
        if (Input.GetButtonDown("Slot1"))
        {
            controllable.OnSlot1();
        }

        if (Input.GetButtonDown("Slot2"))
        {
            controllable.OnSlot2();
        }

        if (Input.GetButtonDown("Slot3"))
        {
            controllable.OnSlot3();
        }

    }


}
