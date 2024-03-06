using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IBoundsProvider
    {
        SpaceBounds Bounds { get; }
    }
}
