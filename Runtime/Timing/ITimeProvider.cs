using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public interface ITimeProvider
    {
        Task<DateTime> GetTimeAsync();
    }
}