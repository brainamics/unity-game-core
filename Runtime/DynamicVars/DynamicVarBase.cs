using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Brainamics.Core
{
	public abstract class DynamicVarBase<TVar, TSelf> : ScriptableObject
		where TSelf : DynamicVarBase<TVar, TSelf>
	{
		internal const string AssetMenuPath = "Game/Dynamic Variables/";

		[SerializeField]
		private TVar _value;

		public UnityEvent<TSelf> OnValueChanged;

		public TVar Value
		{
			get => _value;
			set
			{
				_value = value;
				OnValueChanged.Invoke((TSelf)this);
			}
		}

		private void OnEnable()
		{
			_value = default;
		}
	}
}