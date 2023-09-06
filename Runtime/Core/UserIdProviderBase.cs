using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    // [CreateAssetMenu(menuName = "Game/Services/???")]
    public abstract class UserIdProviderBase : ScriptableObject, IUserIdProvider
    {
        public abstract string GetUserId();
    }
}
