using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void OnMouseMoved();
    void OnHorizontalAxis(float value);
    void OnVerticalAxis(float value);
    void OnLeftAnalogStick(Vector2 tilt);
    void OnRightAnalogStick(Vector2 tilt);
    void OnKeyboardFirePressed(Vector2 mousePosition);
    void OnJoystickFirePressed(Vector2 joystickDirection);
    void OnClutchPressed();
    void OnClutchHeld();
    void OnClutchReleased();
    void OnFireHeld();
    void OnFireReleased();
    void OnJumpPressed(Vector2 tilt);
    void OnSlot1();
    void OnSlot2();
    void OnSlot3();
}
