using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A controller used to control character objects
/// 
/// The Av is added in to not clash with Unity's in house character controller.
/// </summary>
public class AvCharacterController : MonoBehaviour, IControllable
{

    Character character;

    void Start()
    {
        character = GetComponent<Character>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnFireHeld()
    {
        character.OnFireHeld();
    }

    public void OnKeyboardFirePressed(Vector2 mousePosition)
    {
        Vector2 cursorInWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = cursorInWorldPos - (Vector2) transform.position;

        character.Fire(direction);
    }

    public void OnJoystickFirePressed(Vector2 joystickDirection)
    {
        character.Fire(joystickDirection);
    }

    public void OnFireReleased()
    {
        character.OnFireReleased();
    }

    public void OnHorizontalAxis(float tiltValue)
    {
        character.Move(new Vector2(tiltValue, 0));
    }

    public void OnVerticalAxis(float value)
    {
        character.OnVerticalAxis(value);
    }

    public void OnMouseMoved()
    {
        character.PointWithMouse();
    }
    public void OnLeftAnalogStick(Vector2 tilt)
    {
        character.Move(tilt);
    }

    public void OnRightAnalogStick(Vector2 tilt)
    {
        character.PointWithJoystick(tilt);
    }

    public void OnJumpPressed()
    {
        character.Jump();
    }

    public void OnSlot1()
    {
        throw new System.NotImplementedException();
    }

    public void OnSlot2()
    {
        throw new System.NotImplementedException();
    }

    public void OnSlot3()
    {
        throw new System.NotImplementedException();
    }
}
