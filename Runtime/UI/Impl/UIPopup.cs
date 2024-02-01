using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIPopup : MonoBehaviour, IUIPopup
{
    private bool _visible;

    [Header("Events")]
    [SerializeField]
    private UnityEvent<IUIPopup> _onVisibilityChanged;

    public bool IsVisible
    {
        get => _visible;
        set
        {
            if (_visible == value)
                return;
            _visible = value;
            OnVisibilityChanged.Invoke(this);
        }
    }

    public UnityEvent<IUIPopup> OnVisibilityChanged => _onVisibilityChanged;
}
