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
    private InputMode inputMode;

    public enum InputMode
    {
        Keyboard, Controller
    };

    // Start is called before the first frame update
    void Start()
    {
        UpdateInputMode();
        controllable = controllingObject.GetComponent<AvCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInputMode();
        if (inputMode == InputMode.Keyboard)
        {
            CheckKeyboardButtons();
        }

        else if (inputMode == InputMode.Controller)
        {
            CheckControllerButtons();
        }

        if (Input.GetButtonDown("Jump"))
        {
            controllable.OnJumpPressed();
        }

        // Fire handling

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

    void UpdateInputMode()
    {
        if(Input.GetAxis("Mouse X") < 0 || Input.GetAxis("Mouse X") > 0 || Input.GetAxis("Mouse Y") < 0 || Input.GetAxis("Mouse Y") > 0)
        {
           inputMode = InputMode.Keyboard;
        }

        // TODO: Add more arguments
        if (Input.GetAxis("LeftJoyHorizontal") < 0 || 
            Input.GetAxis("LeftJoyHorizontal") > 0 || 
            Input.GetAxis("LeftJoyVertical") < 0 || 
            Input.GetAxis("LeftJoyVertical") > 0 ||
            Input.GetAxis("RightJoyHorizontal") < 0 ||
            Input.GetAxis("RightJoyHorizontal") > 0 ||
            Input.GetAxis("RightJoyVertical") < 0 ||
            Input.GetAxis("RightJoyVertical") > 0)
        {
            inputMode = InputMode.Controller;
        }
    }

    void CheckKeyboardButtons()
    {
        controllable.OnHorizontalAxis(Input.GetAxis("Horizontal"));
        controllable.OnVerticalAxis(Input.GetAxis("Vertical"));
        controllable.OnMouseMoved();

        if (Input.GetButtonDown("Fire1"))
        {
            controllable.OnKeyboardFirePressed(Input.mousePosition);
        }
    }

    void CheckControllerButtons()
    {
        float xTilt = Input.GetAxis("LeftJoyHorizontal");
        float yTilt = Input.GetAxis("LeftJoyVertical");
        controllable.OnLeftAnalogStick(new Vector2(xTilt, yTilt));

        xTilt = Input.GetAxis("RightJoyHorizontal");
        yTilt = Input.GetAxis("RightJoyVertical");
        controllable.OnRightAnalogStick(new Vector2(xTilt, yTilt));

        if (Input.GetAxis("Fire1") != 0)
        {
            // Calculate direction of fire.
            controllable.OnJoystickFirePressed(new Vector2(xTilt, yTilt));
        }

        if (Input.GetButtonDown("Jump"))
        {
            controllable.OnJumpPressed();
        }
    }

    public InputMode GetInputMode()
    {
        return inputMode;
    }
}
