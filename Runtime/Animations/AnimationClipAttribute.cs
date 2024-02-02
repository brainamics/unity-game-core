using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class AnimationClipAttribute : Attribute
    {
        public string MenuName { get; set; }

        public string ResolveMenuName(Type type)
            => string.IsNullOrEmpty(MenuName) ? type.Name : MenuName;
    }
}
