using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class InstantiatedPrefabReference : MonoBehaviour
    {
        [SerializeField]
        public GameObject _prefab;

        public GameObject Prefab
        {
            get => _prefab;
            set
            {
                _prefab = value;
                ValidatePrefab();
            }
        }

        public static InstantiatedPrefabReference For(GameObject obj)
        {
            if (obj == null)
                throw new System.ArgumentNullException(nameof(obj));
            return obj.TryGetComponent<InstantiatedPrefabReference>(out var @ref) ? @ref : obj.AddComponent<InstantiatedPrefabReference>();
        }

        public static GameObject TryGetPrefabReference(GameObject obj)
            => obj != null && obj.TryGetComponent<InstantiatedPrefabReference>(out var @ref) ? @ref.Prefab : null;

        public static GameObject GetPrefabReference(GameObject obj)
        {
            var prefab = TryGetPrefabReference(obj);
            if (prefab == null)
                throw new System.InvalidOperationException("The prefab reference is missing.");
            return prefab;
        }

        private void OnValidate()
        {
            ValidatePrefab();
        }

        private void ValidatePrefab()
        {
            if (_prefab == null || _prefab.scene.name == null)
                return;
            _prefab = null;
            throw new System.InvalidOperationException($"Object '{this}' cannot accept a reference to an instance.");
        }
    }
}
