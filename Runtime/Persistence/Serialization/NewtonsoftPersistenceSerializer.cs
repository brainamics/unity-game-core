using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [CreateAssetMenu(menuName = "Game/Services/Persistence/Newtonsoft Json Serializer")]
    public class NewtonsoftPersistenceSerializer : PersistenceSerializerBase
    {
        public override object Deserialize(string serializedString, Type type)
        {
            return JsonConvert.DeserializeObject(serializedString, type);
        }

        public override T Deserialize<T>(string serializedString)
        {
            return JsonConvert.DeserializeObject<T>(serializedString);
        }

        public override string Serialize(object o)
        {
            return JsonConvert.SerializeObject(o);
        }
    }
}
