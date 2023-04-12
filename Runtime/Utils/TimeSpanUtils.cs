using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public static class TimeSpanUtils
    {
        public static TimeSpan Min(TimeSpan time1, TimeSpan time2)
        {
            return time1 < time2 ? time1 : time2;
        }

        public static TimeSpan Max(TimeSpan time1, TimeSpan time2)
        {
            return time1 > time2 ? time1 : time2;
        }
    }
}