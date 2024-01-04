using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Brainamics.Core
{
    public static class PhoneNumberValidator
    {
        public static bool IsLikelyMobileNumber(string number)
        {
            var pureNumeric = Regex.Replace(number, @"[^\d]", string.Empty).TrimStart('0');
            return pureNumeric.Length >= 10;
        }
    }
}
