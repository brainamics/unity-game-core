using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IUIWindow
    {
        GameObject gameObject { get; }

        bool CanClose { get; }
    }
}
