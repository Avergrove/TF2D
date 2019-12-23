using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Constants
{
    public const int platformLayer = 8;
    public const int characters = 9;

    public const int ProjectileMask = 1 << platformLayer << characters;
}
