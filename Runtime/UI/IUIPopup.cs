using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IUIPopup
    {
        GameObject gameObject { get; }

        bool IsVisible { get; set; }

        UnityEvent<IUIWindow> OnVisibilityChanged { get; }
    }
}
