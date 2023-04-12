using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPersistentState<TState>
{
    void SaveState(TState state);

    void LoadState(TState state);
}
