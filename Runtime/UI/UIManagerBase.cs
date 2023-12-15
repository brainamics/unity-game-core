using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class UIManagerBase : MonoBehaviour
    {
        protected readonly List<IUIWindow> _windows = new();
        protected readonly HashSet<IUIPopup> _popups = new();

        public UnityEvent<IUIWindow> OnWindowOpened, OnWindowClosed, OnWindowVisibilityChanged;
        public UnityEvent<IUIPopup> OnPopupOpened, OnPopupClosed, OnPopupVisibilityChanged;

        public IReadOnlyList<IUIWindow> Windows => _windows;

        public IReadOnlyCollection<IUIWindow> Popups => _popups;

        protected bool AllowMultipleWindows { get; set; } = true;

        protected bool ClosePopupsBeforeOpeningWindows { get; set; } = true;

        public bool TryCloseAll()
        {
            CloseAllPopups();
            return TryCloseAllWindows();
        }

        public bool OpenWindow(IUIWindow window)
        {
            if (!PushWindowPreprocess(window))
                return false;
            if (ClosePopupsBeforeOpeningWindows)
                CloseAllPopups();
            _windows.Push(window);
            SetWindowVisibility(window, true);
            OnWindowOpened.Invoke(window);
            return true;
        }

        public virtual void SetWindowVisibility(IUIWindow window, bool visible)
        {
            window.IsVisible = visible;
            OnWindowVisibilityChanged.Invoke(window);
        }

        public bool TryCloseAllWindows()
        {
            var anyFailed = false;
            while (_windows.TryPop(out var window))
                if (!TryCloseWindow(window))
                    anyFailed = true;
            return !anyFailed;
        }

        public bool TryCloseWindow(IUIWindow window)
        {
            if (!window.CanClose)
                return false;
            CloseWindow(window);
            return true;
        }

        public void OpenPopup(IUIPopup popup)
        {
            _popups.Add(popup);
            SetPopupVisibility(popup, true);
            OnPopupOpened.Invoke(popup);
        }

        public virtual void SetPopupVisibility(IUIPopup popup, bool visible)
        {
            popup.gameObject.SetActive(visible);
            OnPopupVisibilityChanged.Invoke(popup);
        }

        public void CloseAllPopups()
        {
            foreach (var popup in _popups.ToArray())
                ClosePopup(popup);
            _popups.Clear();
        }

        public virtual void ClosePopup(IUIPopup popup)
        {
            SetPopupVisibility(popup, false);
            _popups.Remove(popup);
            OnPopupClosed.Invoke(popup);
        }

        protected virtual bool PushWindowPreprocess(IUIWindow windowObj)
        {
            if (!windowObj.CanClose)
                return false;
            if (!AllowMultipleWindows && !TryCloseAllWindows())
                return false;
            return true;
        }

        protected virtual void CloseWindow(IUIWindow window)
        {
            SetWindowVisibility(window, false);
            _windows.Remove(window);
            OnWindowClosed.Invoke(window);
        }
    }
}
