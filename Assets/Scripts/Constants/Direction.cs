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

    /// <summary>
    /// Converts the enumerable into an signed integer of 1 for easier calculation of direction
    /// Right and up is 1, left and down is -1
    /// </summary>
    /// <param name="facingDirection"></param>
    /// <returns></returns>
    public static int ConvertToIdentityInteger(DirectionEnum facingDirection)
    {
        switch (facingDirection)
        {
            case DirectionEnum.Up:
                return 1;

            case DirectionEnum.Down:
                return -1;

            case DirectionEnum.Left:
                return -1;

            case DirectionEnum.Right:
                return 1;

            default:
                throw new System.Exception("This will definitely not happen.");
        }
    }
}
