using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class GameTime
    {
        public static IGameTime Get(GameObject go)
        {
            var gameTime = go.GetComponentInParent<IGameTime>();
            if (gameTime == null)
                gameTime = DefaultGameTime.Instance;
            return gameTime;
        }
    }
}
