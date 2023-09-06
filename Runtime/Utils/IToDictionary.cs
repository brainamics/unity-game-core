using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public interface IToDictionary
    {
        void ToDictionary(IDictionary<string, object> dictionary);
    }

    public static class ToDictionaryExtensions
    {
        public static Dictionary<string, object> ToDictionary(this IToDictionary td)
        {
            var result = new Dictionary<string, object>();
            td.ToDictionary(result);
            return result;
        }

        public static Dictionary<string, string> ToStringDictionary(this IToDictionary td)
        {
            return td.ToDictionary()
                .ToDictionary(x => x.Key, x => x.Value?.ToString());
        }
    }
}
