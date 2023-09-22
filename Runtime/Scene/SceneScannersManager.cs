using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    public static class SceneScannersManager
    {
        public static event Action<object> OnScannerCreated;
        public static event Action<object> OnScannerDestroying;

        // scene => scene manager
        private static readonly HybridDictionary _managers = new();

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner Locate<TGameSceneScanner>(MonoBehaviour component)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            return LocateByScene<TGameSceneScanner>(component.gameObject.scene);
        }

        /// <summary>
        /// Locates the game scene manager for the scene associated with <paramref name="component"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static TGameSceneScanner TryLocate<TGameSceneScanner>(MonoBehaviour component)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            return TryLocateByScene<TGameSceneScanner>(component.gameObject.scene);
        }

        /// <summary>
        /// Locates the game scene manager for the specified scene.
        /// </summary>
        public static TGameSceneScanner LocateByScene<TGameSceneScanner>(Scene scene)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            return TryLocateByScene<TGameSceneScanner>(scene) ?? throw new InvalidOperationException("Could not find the game scene manager, it's not registered.");
        }

        /// <summary>
        /// Locates the game scene manager for the specified scene.
        /// </summary>
        public static TGameSceneScanner TryLocateByScene<TGameSceneScanner>(Scene scene)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            var manager = _managers[scene];
            return (TGameSceneScanner)manager;
        }

        /// <summary>
        /// Registers a scanner on the manager.
        /// </summary>
        public static void Register<TGameSceneScanner>(TGameSceneScanner scanner)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            var scene = scanner.gameObject.scene;
#if DEBUG
            if (_managers[scene] != null)
                throw new InvalidOperationException("There is another manager associated with the current scene already.");
#endif
            _managers[scene] = scanner;
            OnScannerCreated?.Invoke(scanner);
        }

        /// <summary>
        /// Unregisters a scanner.
        /// </summary>
        public static void Unregister<TGameSceneScanner>(TGameSceneScanner scanner)
            where TGameSceneScanner : GameSceneScannerBase<TGameSceneScanner>
        {
            var scene = scanner.gameObject.scene;
            if (!_managers.Contains(scene))
                return;
            _managers.Remove(scene);
            OnScannerDestroying?.Invoke(scanner);
        }
    }
}
