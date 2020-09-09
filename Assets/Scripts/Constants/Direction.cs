using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Direction
{
    public enum DirectionEnum
    {
        Up, Down, Left, Right
    }

    /// <summary>
    /// Flips the sign of the input based on direction
    /// </summary>
    /// <param name="facingDirection">The direction the object is facing.</param>
    /// <param name="input">The input to flip</param>
    /// <returns>input if Right, -input if Left</returns>
    public static float ConsiderDirection(DirectionEnum facingDirection, float input)
    {
        if (facingDirection == DirectionEnum.Left)
        {
            input = -input;
        }

        return input;
    }
}
