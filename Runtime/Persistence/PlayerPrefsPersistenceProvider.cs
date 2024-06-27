using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    //[CreateAssetMenu(menuName = "Game/Services/Persistence/PlayerPrefs Persistence Provider")]
    //public class PlayerPrefsPersistenceProvider : PlayerPrefsPersistenceProvider<GameStateData> { }

    public class PlayerPrefsPersistenceProvider<TState> : PersistenceProviderBase<TState>
    {
        [SerializeField]
        private PersistenceServiceBase<TState> _persistenceService;

        [SerializeField]
        private PersistenceSerializerBase _serializer;


        [SerializeField]
        private string _key = "SAVEDATA";

        public override Task<TCustomState> LoadStateAsync<TCustomState>()
        {
            return Task.FromResult(LoadStateFromPrefs<TCustomState>());
        }

        public override Task<TState> LoadStateAsync()
        {
            return Task.FromResult(LoadStateFromPrefs());
        }

        public override Task SaveStateAsync(TState state)
        {
            SaveStateToPrefs(state);
            return Task.CompletedTask;
        }

        private TCustomState LoadStateFromPrefs<TCustomState>()
        {
            try
            {
                var data = PlayerPrefs.GetString(_key);
                if (string.IsNullOrEmpty(data))
                {
                    return default;
                }

                return _serializer.Deserialize<TCustomState>(data);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private TState LoadStateFromPrefs()
        {
            return LoadStateFromPrefs<TState>();
        }

        private void SaveStateToPrefs(TState state)
        {
            PlayerPrefs.SetString(_key, _serializer.Serialize(state));
        }
    }
}
