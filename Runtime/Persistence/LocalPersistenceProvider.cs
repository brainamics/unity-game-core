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
        private PersistenceSerializerBase _serializer;

        [SerializeField]
        private string _fileName = "savedata";

        public string FilePath => _filePath;

        public override Task<TCustomState> LoadStateAsync<TCustomState>()
        {
            return Task.FromResult(LoadStateFromFile<TCustomState>());
        }

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
            EnsureDirectory();
        }

        private TCustomState LoadStateFromFile<TCustomState>()
        {
            try
            {
                if (!File.Exists(_filePath))
                    return default;
                var data = File.ReadAllText(_filePath);
                if (_serializer == null)
                    return JsonConvert.DeserializeObject<TCustomState>(data);
                return _serializer.Deserialize<TCustomState>(data);
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

        private TState LoadStateFromFile()
            => LoadStateFromFile<TState>();

        private void SaveStateToFile(TState state)
        {
            EnsureDirectory();
            string data;
            if (_serializer == null)
                data = JsonConvert.SerializeObject(state);
            else
                data = _serializer.Serialize(state);
            File.WriteAllText(_filePath, data);
        }

        private void EnsureDirectory()
        {
            var dirName = Path.GetDirectoryName(_filePath);
            if (!Directory.Exists(dirName))
                Directory.CreateDirectory(dirName);
        }
    }
}
