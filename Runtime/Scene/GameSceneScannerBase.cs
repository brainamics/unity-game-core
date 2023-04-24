using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    /// <summary>
    /// Stores references to important objects or components on the scene.
    /// </summary>
    public abstract class GameSceneScannerBase<TGameSceneManager> : MonoBehaviour
        where TGameSceneManager : GameSceneScannerBase<TGameSceneManager>
    {
        // scene => scene manager
        private static readonly HybridDictionary _managers = new();

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneManager Locate(MonoBehaviour component)
        {
            return LocateByScene(component.gameObject.scene);
        }

        /// <summary>
        /// Locates the game scene manager for the specified scene.
        /// </summary>
        public static TGameSceneManager LocateByScene(Scene scene)
        {
            var manager = _managers[scene];
            if (manager == null)
                throw new InvalidOperationException("Could not find the game scene manager, it's not registered.");
            return (TGameSceneManager)manager;
        }

        protected virtual void AwakeInternal() { }

        protected virtual void StartInternal() { }

        protected virtual void OnDestroyInternal() { }

        private void Awake()
        {
            var scene = gameObject.scene;
#if DEBUG
            if (_managers[scene] != null)
                throw new InvalidOperationException("There is another manager associated with the current scene already.");
#endif
            _managers[scene] = this;
            AwakeInternal();
        }

        private void Start()
        {
            StartInternal();
        }

        private void OnDestroy()
        {
            _managers.Remove(gameObject.scene);
        }
    }
}
