using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class MobileGameLoaderBase : MonoBehaviour
    {
        private static MobileGameLoaderBase Instance;

        [SerializeField]
        [Tooltip("Overrides max frame rate for the game")]
        private int _targetFrameRate = 0;

        [SerializeField]
        private bool _enablePhysicsSimulation = true;

        public bool IsPaused { get; private set; }

        protected abstract void Initialize();

        protected virtual void PauseStateChanged(bool paused)
        {
        }

        private void Awake()
        {
            if (_targetFrameRate > 0)
                Application.targetFrameRate = _targetFrameRate;
            if (!_enablePhysicsSimulation)
                // Physics.simulationMode = _enablePhysicsSimulation ? SimulationMode.FixedUpdate : SimulationMode.Script;
                Physics.simulationMode = SimulationMode.Script;
            if (Instance != null)
                Destroy(Instance.gameObject);
            Instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            Initialize();
        }

        private void OnApplicationPause(bool pause)
        {
            SetPause(pause);
        }

        private void OnApplicationFocus(bool focus)
        {
            SetPause(!focus);
        }

        private void SetPause(bool paused)
        {
            if (IsPaused == paused)
                return;
            PauseStateChanged(paused);
        }
    }
}
