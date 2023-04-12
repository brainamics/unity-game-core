using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{

    public abstract class PersistenceProviderBase<TState> : ScriptableObject, IPersistenceProvider<TState>
    {
        public abstract Task<TState> LoadStateAsync();

        public abstract Task SaveStateAsync(TState state);
    }
}