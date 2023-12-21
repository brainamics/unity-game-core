using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    public static class PersistentIdTrackerManager
    {
        private static readonly Dictionary<Scene, PersistentIdTracker> _trackersMap = new();

        static PersistentIdTrackerManager()
        {
            SceneManager.sceneUnloaded += SceneManager_sceneUnloaded;
        }

        private static Dictionary<Scene, PersistentIdTracker> TrackersMap => _trackersMap;

        public static PersistentIdTracker TryGetTracker(Scene scene)
        {
            return TrackersMap.GetValueOrDefault(scene);
        }

        public static PersistentIdTracker Obtain(Scene scene)
        {
            if (TrackersMap.TryGetValue(scene, out var tracker))
                return tracker;
            return RegisterScene(scene);
        }

        public static PersistentIdTracker Obtain(GameObject obj)
            => obj.scene == null ? null : Obtain(obj.scene);

        private static PersistentIdTracker RegisterScene(Scene scene)
        {
            var tracker = new PersistentIdTracker();
            TrackersMap[scene] = tracker;
            return tracker;
        }

        private static void UnregisterScene(Scene scene)
        {
            TrackersMap.Remove(scene);
        }

        private static void SceneManager_sceneUnloaded(Scene scene)
        {
            UnregisterScene(scene);
        }
    }
}