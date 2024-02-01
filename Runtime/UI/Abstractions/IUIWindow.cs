using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IUIWindow
    {
        GameObject gameObject { get; }

        bool CanClose { get; }

        bool IsVisible { get; set; }

        UnityEvent<IUIWindow> OnVisibilityChanged { get; }
    }
}
