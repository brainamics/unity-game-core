using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class LocalTimeProvider : ITimeProvider
{
    public Task<DateTime> GetTimeAsync()
    {
        return Task.FromResult(DateTime.UtcNow);
    }
}
