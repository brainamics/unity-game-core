using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class DefaultGameTime : IGameTime
    {
        private static DefaultGameTime _instance;

        public static IGameTime Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                lock (typeof(DefaultGameTime))
                {
                    if (_instance != null)
                        return _instance;
                    _instance = new DefaultGameTime();
                }

                return _instance;
            }
        }

        public bool IsUnscaled => false;

        public float TimeScale
        {
            get => UnityEngine.Time.timeScale;
            set => UnityEngine.Time.timeScale = value;
        }

        public float Time => UnityEngine.Time.time;

        public double TimeAsDouble => UnityEngine.Time.timeAsDouble;

        public float DeltaTime => UnityEngine.Time.deltaTime;

        public float UnscaledTime => UnityEngine.Time.unscaledTime;

        public double UnscaledTimeAsDouble => UnityEngine.Time.unscaledTimeAsDouble;

        public float UnscaledDeltaTime => UnityEngine.Time.unscaledDeltaTime;
    }
}
