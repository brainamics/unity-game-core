using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public class PersistentIdTracker
    {
        private readonly Dictionary<string, PersistentId> _ids = new();
        private readonly HashSet<PersistentId> _persistentIds = new();

        public PersistentId GetRegisteredPersistentId(string id)
            => _ids.GetValueOrDefault(id);

        public bool Track(PersistentId id)
        {
#if !UNITY_EDITOR
            return TrackInternal(id);
#else
            return Application.isPlaying || TrackInternal(id);
#endif
        }

        public bool Untrack(PersistentId id)
        {
#if !UNITY_EDITOR
        return UntrackInternal(id);
#else
            return Application.isPlaying || UntrackInternal(id);
#endif
        }

        public void TrackOrThrow(PersistentId id)
        {
            if (Track(id))
                return;
            throw new System.InvalidOperationException($"The persistent ID is duplicate: {id}");
        }

        private bool TrackInternal(PersistentId id)
        {
            if (!_persistentIds.Add(id))
                return true;
            var success = _ids.TryAdd(id.Id, id);
            if (!success)
                _persistentIds.Remove(id);
            return success;
        }

        private bool UntrackInternal(PersistentId id)
        {
            if (!_persistentIds.Remove(id))
                return false;
            if (!_ids.TryGetValue(id.RegisteredId, out var actualId))
                return true;
            if (actualId != id)
                return true;
            _ids.Remove(id.RegisteredId);
            return true;
        }
    }
}
