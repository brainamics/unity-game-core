using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    /// <summary>
    /// Defines all the possible presentation modes for an ad.
    /// </summary>
    public enum AdPresentationMode
    {
        /// <summary>
        /// The ad will be exclusively displayed on the screen, resulting in the game getting paused, and ultimately focusing solely on the ads.
        /// </summary>
        Exclusive,

        /// <summary>
        /// The ad will be simultaneously displayed on the game screen as the game, or as a part of the game.
        /// </summary>
        Concurrent,
    }
}
