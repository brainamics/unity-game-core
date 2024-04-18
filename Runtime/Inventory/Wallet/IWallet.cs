using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public interface IWallet<TCurrency> : IEnumerable<ValueTuple<TCurrency, BigInteger>>
    {
        event WalletCurrencyChangeHandler<TCurrency> OnCurrencyChanged;

        /// <summary>
        /// Gets or sets the balance of the specified currency.
        /// </summary>
        BigInteger this[TCurrency currency] { get; set; }

        /// <summary>
        /// Clears the wallet.
        /// </summary>
        void Clear();

        BigInteger GetBalance(TCurrency currency);

        void SetBalance(TCurrency currency, BigInteger value);

        BigInteger AddBalance(TCurrency currency, BigInteger amount);

        bool TryUseBalance(TCurrency currency, BigInteger amount);

        void UseBalance(TCurrency currency, BigInteger amount);
    }
}
