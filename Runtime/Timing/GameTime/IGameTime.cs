using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IGameTime
    {
        bool IsUnscaled { get; }

        float TimeScale { get; set; }

        float Time { get; }

        double TimeAsDouble { get; }

        float DeltaTime { get; }

        float UnscaledDeltaTime { get; }

        float UnscaledTime { get; }

        double UnscaledTimeAsDouble { get; }
    }
}
