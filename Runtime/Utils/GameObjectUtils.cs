using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class GameObjectUtils
    {
        public static int GetLayer(this LayerMask mask)
        {
            return (int)Mathf.Log(mask.value, 2);
        }
        
        public static GameObject GetGameObject(object o)
        {
            if (o == null)
                return null;
            return o switch
            {
                Transform t => t.gameObject,
                GameObject obj => obj,
                Component c => c.gameObject,
                _ => null,
            };
        }
    
        public static GameObject GetGameObjectOrThrow(object o)
        {
            if (o == null)
                return null;
            var go = GetGameObject(o);
            if (go == null)
                throw new System.InvalidOperationException($"Cannot resolve GameObject from an object of type '{o.GetType()}'.");
            return go;
        }
    
        public static Transform GetTransform(object o)
        {
            var go = GetGameObject(o);
            return go == null ? null : go.transform;
        }
    
        public static Transform GetTransformOrThrow(object o)
        {
            if (o == null)
                return null;
            var transform = GetTransform(o);
            if (transform == null)
                throw new System.InvalidOperationException($"Cannot resolve GameObject from an object of type '{o.GetType()}'.");
            return transform;
        }
    }
}
