using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [Flags]
    public enum CoordinateMode
    {
        None = 0,
        World = 1,
        Screen = 2,
        Viewport = 4,
    }
}
