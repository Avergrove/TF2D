using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IControllable
{
    void OnHorizontalAxis(float value);
    void OnVerticalAxis(float value);
    void OnFirePressed();
    void OnJumpPressed();
}
