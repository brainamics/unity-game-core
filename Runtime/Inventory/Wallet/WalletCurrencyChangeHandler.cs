using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

namespace Brainamics.Core
{
    public delegate void WalletCurrencyChangeHandler<TCurrency>(IWallet<TCurrency> wallet, TCurrency currency, BigInteger oldValue, BigInteger newValue);
}
