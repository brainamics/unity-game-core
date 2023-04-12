using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public interface ITimeProvider
{
    Task<DateTime> GetTimeAsync();
}
