using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IPersistenceProvider<TState>
    {
        Task<TCustomState> LoadStateAsync<TCustomState>();

        Task<TState> LoadStateAsync();

        Task SaveStateAsync(TState state);
    }
}