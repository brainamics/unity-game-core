using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public delegate void DraggingUpdateDelegate(DraggingContext context);

    public delegate bool DraggingZoneAcceptDelegate(DraggingContext context);
}