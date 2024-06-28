using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
	[CreateAssetMenu(menuName = "Game/Services/Persistence/Unity Json Serializer")]
	public class JsonPersistenceSerializer : PersistenceSerializerBase
	{
		public override object Deserialize(string serializedString, Type type)
		{
			return JsonUtility.FromJson(serializedString, type);
		}

		public override T Deserialize<T>(string serializedString)
		{
			return JsonUtility.FromJson<T>(serializedString);
		}

		public override string Serialize(object o)
		{
			return JsonUtility.ToJson(o);
		}
	}
}
