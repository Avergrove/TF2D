using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Represents a player's controllers, can be hooked up the controllable objects.
 */
public class InputManager : MonoBehaviour
{

    public GameObject controllingObject;
    public GameObject optionsHandler;
    
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

        float moveXTilt = Input.GetAxis("Horizontal");
        float moveYTilt = Input.GetAxis("Vertical");

        controllable.OnHorizontalAxis(moveXTilt);
        controllable.OnVerticalAxis(moveYTilt);

        controllable.OnMouseMoved();

        if (Input.GetButtonDown("Fire1"))
        {
            controllable.OnKeyboardFirePressed(Input.mousePosition);
        }

        if (Input.GetButtonDown("Jump"))
        {
            controllable.OnJumpPressed(new Vector2(moveXTilt, moveYTilt));
        }

        if (Input.GetButtonDown("Pause"))
        {
            Application.Quit();
        }
    }

    void CheckControllerButtons()
    {
        float leftXTilt = Input.GetAxis("LeftJoyHorizontal");
        float leftYTilt = Input.GetAxis("LeftJoyVertical");
        controllable.OnLeftAnalogStick(new Vector2(leftXTilt, leftYTilt));

        float rightXTilt = Input.GetAxis("RightJoyHorizontal");
        float rightYTilt = Input.GetAxis("RightJoyVertical");
        controllable.OnRightAnalogStick(new Vector2(rightXTilt, rightYTilt));

        if (Input.GetButtonDown("Jump"))
        {
            controllable.OnJumpPressed(new Vector2(leftXTilt, leftYTilt));
        }

        if (Input.GetButton("Fire1"))
        {
            // Calculate direction of fire.
            controllable.OnJoystickFirePressed(new Vector2(rightXTilt, rightYTilt));
        }
    }

    public InputMode GetInputMode()
    {
        return inputMode;
    }


}
