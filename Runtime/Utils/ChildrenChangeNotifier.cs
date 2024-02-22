using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class ChildrenChangeNotifier : MonoBehaviour
    {
        private int _childrenCount;

        public UnityEvent<ChildrenChangeNotifier> OnChildrenChanged;

        public static ChildrenChangeNotifier Obtain(Transform transform)
        {
            if (transform.TryGetComponent<ChildrenChangeNotifier>(out var notifier))
                return notifier;

            return transform.gameObject.AddComponent<ChildrenChangeNotifier>();
        }

        private void OnEnable()
        {
            UpdateChildrenCount();
        }

        private void Update()
        {
            UpdateState();
        }

        private void UpdateState()
        {
            var count = transform.childCount;
            if (count == _childrenCount)
                return;

            UpdateChildrenCount();
            OnChildrenChanged.Invoke(this);
        }

        private void UpdateChildrenCount()
        {
            _childrenCount = transform.childCount;
        }
    }
}
