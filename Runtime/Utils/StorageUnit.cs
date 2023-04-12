using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    internal sealed class StorageUnit
    {
        private int _amount, _capacity;

        public int Capacity
        {
            get => _capacity;
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));
                _capacity = value;
            }
        }

        public int Amount => _amount;

        public bool TryStore(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (amount == 0)
                return true;
            var newAmount = _amount + amount;
            if (newAmount > _capacity)
                return false;
            _amount = newAmount;
            return true;
        }

        public int TryTake(int amount)
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException(nameof(amount));
            if (amount == 0)
                return 0;
            amount = Math.Min(_amount, amount);
            _amount -= amount;
            return amount;
        }

        public void Store(int amount)
        {
            if (!TryStore(amount))
                throw new InvalidOperationException($"The storage unit has insufficient capacity for storing {amount} unit(s).");
        }

        public void Take(int amount)
        {
            var taken = TryTake(amount);
            if (taken == amount)
                return;
            Store(taken);
            throw new InvalidOperationException($"Could not take the required number of units from the storage; {taken} unit(s) were taken instead of the requested {amount} unit(s).");
        }
    }
}