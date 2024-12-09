using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    /// <summary>
    /// Stores references to important objects or components on the scene.
    /// </summary>
    [DefaultExecutionOrder(-99)]
    public abstract class GameSceneScannerBase<TGameSceneScanner> : MonoBehaviour
        where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
    {
        public event Action<TGameSceneScanner> OnDestroying;

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner Locate(GameObject go)
            => SceneScannersManager.Locate<TGameSceneScanner>(go);

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner Locate(MonoBehaviour component)
            => SceneScannersManager.Locate<TGameSceneScanner>(component);

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner TryLocate(MonoBehaviour component)
            => SceneScannersManager.TryLocate<TGameSceneScanner>(component);

        /// <summary>
        /// Locates the game scene manager for the specified scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner LocateByScene(Scene scene)
            => SceneScannersManager.LocateByScene<TGameSceneScanner>(scene);

        /// <summary>
        /// Locates the game scene manager for the specified scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner TryLocateByScene(Scene scene)
            => SceneScannersManager.TryLocateByScene<TGameSceneScanner>(scene);

        /// <summary>
        /// Locates the game scene manager for the active scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner LocateByActiveScene()
            => LocateByScene(SceneManager.GetActiveScene());

        protected virtual void AwakeInternal() { }

        protected virtual void StartInternal() { }

        protected virtual void OnDestroyInternal() { }

        protected virtual void Awake()
        {
            SceneScannersManager.Register((TGameSceneScanner)this);
            AwakeInternal();
        }

        protected virtual void OnEnable()
        {
            SceneScannersManager.Register((TGameSceneScanner)this);
        }

        protected virtual void OnDisable()
        {
        }

        protected virtual void Start()
        {
            StartInternal();
        }

        protected virtual void OnDestroy()
        {
            OnDestroying?.Invoke((TGameSceneScanner)this);
            SceneScannersManager.Unregister((TGameSceneScanner)this);
        }
    }
}
