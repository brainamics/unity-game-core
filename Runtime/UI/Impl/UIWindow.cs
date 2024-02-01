using Brainamics.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class UIWindow : MonoBehaviour, IUIWindow
    {
        private bool _visible;

        [SerializeField]
        private bool _canClose = true;

        [Header("Optional")]
        [SerializeField]
        protected UIManagerBase _uiManager;

        [Header("Events")]
        [SerializeField]
        private UnityEvent<IUIWindow> _onVisibilityChanged;

        public bool CanClose
        {
            get => _canClose;
            set => _canClose = value;
        }

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

        public UnityEvent<IUIWindow> OnVisibilityChanged => _onVisibilityChanged;

        public virtual void Close()
        {
            if (_uiManager == null)
                return;
            _uiManager.TryCloseWindow(this);
        }

        protected virtual void Awake()
        {
            if (_uiManager == null)
                _uiManager = GetComponentInParent<UIManagerBase>();
        }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }
    }
}