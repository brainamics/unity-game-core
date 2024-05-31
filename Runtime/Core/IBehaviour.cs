using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE1006:Naming Styles")]
    public interface IBehaviour
    {
        bool enabled { get; set; }

        Transform transform { get; }

        GameObject gameObject { get; }

        bool isActiveAndEnabled { get; }
    }

    public static class BehaviourExtensions
    {
        public static Behaviour AsBehaviour(this IBehaviour b)
            => (Behaviour)b;

        public static MonoBehaviour AsMonoBehaviour(this IBehaviour b)
            => (MonoBehaviour)b;
    }
}
