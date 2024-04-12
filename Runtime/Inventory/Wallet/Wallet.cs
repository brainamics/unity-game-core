using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public class Wallet<TCurrency> : IWallet<TCurrency>
    {
        private readonly HybridDictionary _infoMap = new();

        public event WalletCurrencyChangeHandler<TCurrency> OnCurrencyChanged;

        public BigInteger this[TCurrency currency]
        {
            get => GetBalance(currency);
            set => SetBalance(currency, value);
        }

        public BigInteger GetBalance(TCurrency currency)
            => GetInfo(currency).Balance;

        public bool CanAfford(TCurrency currency, BigInteger cost)
            => GetBalance(currency) >= cost;

        public void SetBalance(TCurrency currency, BigInteger value)
        {
            if (value < 0)
                throw new System.ArgumentOutOfRangeException(nameof(value));
            var info = GetInfo(currency);
            var oldValue = info.Balance;
            if (oldValue == value)
                return;
            var cancel = false;
            HandleSettingBalance(info, value, ref cancel);
            if (cancel)
                return;
            info.Balance = value;
            OnCurrencyChanged?.Invoke(this, currency, oldValue, value);
        }

        public BigInteger AddBalance(TCurrency currency, BigInteger amount)
        {
            var result = this[currency] + amount;
            SetBalance(currency, result);
            return result;
        }

        public bool TryUseBalance(TCurrency currency, BigInteger amount)
        {
            var remaining = this[currency];
            if (remaining < amount)
                return false;
            this[currency] -= amount;
            return true;
        }

        public void UseBalance(TCurrency currency, BigInteger amount)
        {
            if (TryUseBalance(currency, amount))
                return;
            throw new System.InvalidOperationException($"Insufficient {currency} balance.");
        }

        public IEnumerator<(TCurrency, BigInteger)> GetEnumerator()
        {
            foreach (var currency in _infoMap)
            {
                var info = (CurrencyInfo)_infoMap[currency];
                yield return (info.Currency, info.Balance);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private CurrencyInfo GetInfo(TCurrency currency)
            => (CurrencyInfo)(_infoMap[currency] ??= new CurrencyInfo(currency));

        protected virtual void HandleSettingBalance(CurrencyInfo info, BigInteger value, ref bool cancel)
        {
        }

        protected sealed class CurrencyInfo
        {
            public TCurrency Currency;
            public BigInteger Balance;

            public CurrencyInfo(TCurrency currency)
            {
                Currency = currency;
            }
        }
    }
}
