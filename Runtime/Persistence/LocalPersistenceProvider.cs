using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Brainamics.Core
{
    // [CreateAssetMenu(menuName = "Game/Services/Persistence/Local Persistence Provider")]
    public abstract class LocalPersistenceProvider<TState> : PersistenceProviderBase<TState>
    {
        private string _filePath;

        [SerializeField]
        private PersistenceServiceBase<TState> _persistenceService;

        [SerializeField]
        private string _fileName = "savedata";

        public override Task<TState> LoadStateAsync()
        {
            return Task.FromResult(LoadStateFromFile());
        }

        public override Task SaveStateAsync(TState state)
        {
            SaveStateToFile(state);
            return Task.CompletedTask;
        }

        private void Awake()
        {
            _filePath = Path.Combine(Application.persistentDataPath, _fileName);
        }

        private TState LoadStateFromFile()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return default;
                return JsonConvert.DeserializeObject<TState>(File.ReadAllText(_filePath));
            }
            catch (Exception)
            {
#if DEBUG
                throw;
#else
                return default;
#endif
            }
        }

        private void SaveStateToFile(TState state)
        {
            File.WriteAllText(_filePath, JsonConvert.SerializeObject(state));
        }
    }
}