using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IUserIdProvider
    {
        string GetUserId();
    }
}
