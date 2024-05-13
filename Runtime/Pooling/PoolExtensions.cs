using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class PoolExtensions
    {
        public static void Destroy(this GameObject obj)
        {
            if (obj == null)
                return;
            if (obj.TryGetComponent<IDestroyable>(out var destroyable))
            {
                destroyable.Destroy();
                return;
            }
            Object.Destroy(obj);
        }

        public static void DestroyObject(this Transform transform)
        {
            transform.gameObject.Destroy();
        }

        public static void DestroyObject(this MonoBehaviour behaviour)
        {
            if (behaviour != null)
                behaviour.gameObject.Destroy();
        }
    }
}
