using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public abstract class PersistenceSerializerBase : ScriptableObject
    {
        public abstract string Serialize(object o);

        public abstract object Deserialize(string serializedString, Type type);

        public abstract T Deserialize<T>(string serializedString);
    }
}
