using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IGameObjectPool
    {
        int Capacity { get; set; }

        void TrimExcess();

        void Reserve(int count);

        GameObject Rent();

        void Return(GameObject obj);
    }
}
