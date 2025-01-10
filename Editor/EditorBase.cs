using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Brainamics.Core
{
    public abstract class EditorBase<T> : Editor
        where T : Object
    {
        public T Target => (T)target;

        public IEnumerable<T> Targets => targets.Cast<T>();

        protected virtual void Awake()
        {
            Initialize();
        }

        protected virtual void OnEnable()
        {
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void Initialize()
        {
        }
    }
}
