using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IInventory
    {
        IEnumerable<IInventoryItem> Items { get; }

        UnityEvent<IInventory, IInventoryItem> OnItemAdded { get; }

        UnityEvent<IInventory, IInventoryItem> OnItemRemoved { get; }

        bool TryAdd(IInventoryItem item);

        bool Remove(IInventoryItem item);

        void Clear();

        void RegisterAddInterceptor(InventoryItemInterceptionDelegate intercept);

        void UnregisterAddInterceptor(InventoryItemInterceptionDelegate intercept);
    }
}
