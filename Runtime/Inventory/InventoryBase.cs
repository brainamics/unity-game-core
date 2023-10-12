using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class InventoryBase : ScriptableObject, IInventory
    {
        private readonly List<IInventoryItem> _items = new();
        private readonly List<InventoryItemInterceptionDelegate> _addInterceptors = new();

        IList<IInventoryItem> Items => _items;

        IEnumerable<IInventoryItem> IInventory.Items => Items;

        [SerializeField]
        private UnityEvent<IInventory, IInventoryItem> _onItemAdded, _onItemRemoved;

        public UnityEvent<IInventory, IInventoryItem> OnItemAdded => _onItemAdded;

        public UnityEvent<IInventory, IInventoryItem> OnItemRemoved => _onItemRemoved;

        public virtual void RegisterAddInterceptor(InventoryItemInterceptionDelegate intercept)
        {
            if (intercept == null)
                throw new System.ArgumentNullException(nameof(intercept));
            _addInterceptors.Add(intercept);
        }

        public virtual void UnregisterAddInterceptor(InventoryItemInterceptionDelegate intercept)
        {
            if (intercept == null)
                throw new System.ArgumentNullException(nameof(intercept));
            _addInterceptors.Remove(intercept);
        }

        public bool TryAdd(IInventoryItem item)
        {
            if (!InterceptAdd(item))
                return false;
            _items.Add(item);
            OnItemAdded.Invoke(this, item);
            return true;
        }

        public bool Remove(IInventoryItem item)
        {
            if (item == null)
                throw new System.ArgumentNullException(nameof(item));
            if (!_items.Remove(item))
                return false;
            OnItemRemoved.Invoke(this, item);
            return true;
        }

        public void Clear()
        {
            _items.Clear();
            OnItemRemoved.Invoke(this, null);
        }

        protected virtual void Awake() { }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy() { }

        protected virtual bool InterceptAdd(IInventoryItem item)
        {
            foreach (var interceptor in _addInterceptors)
                if (!interceptor(item))
                    return false;
            return true;
        }
    }
}
