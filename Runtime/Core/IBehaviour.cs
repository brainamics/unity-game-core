using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
    public interface IBehaviour
    {
        Transform transform { get; }

        GameObject gameObject { get; }
    }
}
