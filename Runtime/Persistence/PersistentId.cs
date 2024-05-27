using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Brainamics.Core
{
    [ExecuteAlways]
    [DefaultExecutionOrder(-50)]
    public sealed class PersistentId : MonoBehaviour
    {
        private PersistentIdTracker _registeredTracker;
        private string _registeredId;

        [ContextMenuItem("New ID", nameof(GenerateId))]
        [SerializeField]
        private string _id;

        public string Id
        {
            get => _id;
            set => _id = value;
        }

        public string RegisteredId => _registeredId;

        public bool IsValid { get; private set; }

        public bool IsApplicable => gameObject.scene.buildIndex >= 0;

        public System.Guid Guid => System.Guid.Parse(_id);

        private bool IsRegistered => _registeredTracker != null;

        public PersistentIdTracker Tracker { get; private set; }

        public static PersistentId TryFind(GameObject obj)
        {
            return obj.GetComponentInParent<PersistentId>(true);
        }

        public static PersistentId Find(GameObject obj)
        {
            var id = TryFind(obj);
            if (id == null)
                throw new System.InvalidOperationException("Could not find a persistent ID for this object.");
            return id;
        }

        public void GenerateNewId()
            => GenerateId();

        public override string ToString()
        {
            return Id;
        }

        private void Awake()
        {
            if (!IsApplicable)
                return;
            UpdateTracker();
            if (Tracker == null)
                throw new System.InvalidOperationException($"Could not locate the {nameof(PersistentIdTracker)} object.");
        }

        private void OnValidate()
        {
            UpdateRegistration();
        }

        private void OnEnable()
        {
            UpdateRegistration();
        }

        private void OnDisable()
        {
            Unregister();
        }

        private void UpdateTracker()
        {
            Tracker = PersistentIdTrackerManager.Obtain(gameObject);
        }

        private void UpdateId()
        {
            if (string.IsNullOrEmpty(_id))
                GenerateId();
            UpdateRegistration();
        }

        private void GenerateId()
        {
            if (!IsApplicable)
                return;
            _id = System.Guid.NewGuid().ToString();
#if UNITY_EDITOR
            EditorUtility.SetDirty(this);
            Undo.RecordObject(this, "Generate Persistent ID");
#endif
            UpdateRegistration();
        }

        private void UpdateRegistration()
        {
            if (!enabled)
            {
                Unregister();
                return;
            }
            if (!IsApplicable && !string.IsNullOrEmpty(_id))
            {
                _id = null;
                return;
            }
            UpdateTracker();
            if (_id == _registeredId && Tracker == _registeredTracker)
                return;

            Unregister();
            try
            {
                IsValid = false;
                Register();
            }
            catch (System.InvalidOperationException)
            {
                throw;
            }
        }

        private void Register()
        {
            if (IsRegistered)
                throw new System.InvalidOperationException("Already registered.");
#if UNITY_EDITOR
            if (BuildPipeline.isBuildingPlayer)
                return;
#endif
            if (string.IsNullOrEmpty(_id))
            {
                IsValid = false;
                return;
            }

            Tracker.TrackOrThrow(this);
            _registeredTracker = Tracker;
            _registeredId = _id;
            IsValid = true;
        }

        private void Unregister()
        {
            if (!IsRegistered)
                return;

            IsValid = false;
            _registeredTracker.Untrack(this);
            _registeredId = null;
            _registeredTracker = null;
        }
    }
}
