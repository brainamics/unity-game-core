using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Brainamics.Core
{
    public static class NameUtility
    {
        private static readonly Regex NameWithNumberRegex = new(@"^(.*?)(\d+)$", RegexOptions.Compiled);

        public static string GetUniqueChildName(Transform transform, string baseName)
        {
            // If the base name is empty, start with "Item" as the base.
            if (string.IsNullOrEmpty(baseName))
            {
                baseName = string.Empty;
            }

            var match = NameWithNumberRegex.Match(baseName);

            // Start with baseName if no number is found, or strip the number to increment it.
            var nameWithoutNumber = match.Success ? match.Groups[1].Value : baseName;
            var startingNumber = match.Success ? int.Parse(match.Groups[2].Value) : 0;

            var uniqueName = startingNumber > 0 ? baseName : $"{baseName}1";
            var unique = false;
            var number = startingNumber;

            // Check if the child with the constructed name exists. If so, increment the number.
            while (!unique)
            {
                unique = true;
                foreach (var child in transform.EnumerateChildren())
                {
                    if (child.name != uniqueName)
                        continue;
                    unique = false;
                    number++;
                    uniqueName = $"{nameWithoutNumber}{number}";
                    break;
                }
            }

            return uniqueName;
        }
    }
}
