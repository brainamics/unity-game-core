using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
    public abstract class MobileGameLoaderBase : MonoBehaviour
    {
        private static MobileGameLoaderBase Instance;
        private readonly List<Task> _startupTasks = new();

        [SerializeField]
        [Tooltip("Overrides max frame rate for the game")]
        private int _targetFrameRate = 0;

        [SerializeField]
        private bool _enablePhysicsSimulation = true;

        public UnityEvent OnInitialize;

        public bool IsPaused { get; private set; }

        public IEnumerable<Task> StartupTasks => _startupTasks;

        public void AddStartupTask(Task task)
        {
            _startupTasks.Add(task);
        }

        public Task WaitForStartupTasksAsync()
        {
            var result = Task.WhenAll(_startupTasks);
            result.ContinueWith(_ =>
            {
                _startupTasks.Clear();
            });
            return result;
        }

        protected abstract void Initialize();

        protected virtual void PauseStateChanged(bool paused)
        {
        }

        private void Awake()
        {
            if (_targetFrameRate > 0)
                Application.targetFrameRate = _targetFrameRate;
            if (!_enablePhysicsSimulation)
                Physics.simulationMode = SimulationMode.Script;
            if (Instance != null)
                Destroy(Instance.gameObject);
            Instance = this;
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            Initialize();
            OnInitialize.Invoke();
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
