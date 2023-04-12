using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Brainamics.Core
{
    public static class UnitConverter
    {
        private static readonly IReadOnlyList<string> StaticUnits = new[] { string.Empty, "K", "M", "B", "T", "Q" };
        private static readonly IReadOnlyList<string> Units;
        private static readonly IReadOnlyDictionary<string, BigInteger> UnitMultipliers;
        private static readonly Regex ParseRegex = new(@"^(\-?)(\d+)(\.(\d+))?\s*(\w*)$", RegexOptions.Compiled);

        static UnitConverter()
        {
            var units = new List<string>(1000);
            units.AddRange(StaticUnits);
            for (var ch1 = 'a'; ch1 <= 'z'; ch1++)
                for (var ch2 = 'a'; ch2 <= 'z'; ch2++)
                    units.Add($"{ch1}{ch2}");
            Units = units;

            var multipliers = new Dictionary<string, BigInteger>(StringComparer.OrdinalIgnoreCase);
            var mul = BigInteger.One;
            foreach (var unit in units)
            {
                multipliers[unit] = mul;
                mul *= 1000;
            }
            UnitMultipliers = multipliers;
        }

        public static BigInteger Parse(string str)
        {
            var match = ParseRegex.Match(str);
            if (!match.Success)
                throw new InvalidOperationException($"Cannot convert to BigInteger: {str}");
            var negative = !string.IsNullOrEmpty(match.Groups[1].Value);
            var numPart = BigInteger.Parse(match.Groups[2].Value);
            var floatPart = string.IsNullOrEmpty(match.Groups[3].Value) ? 0d : double.Parse($"0.{match.Groups[4].Value}");
            var unitPart = match.Groups[5].Value;
            if (!UnitMultipliers.TryGetValue(unitPart, out var unitMul))
                throw new InvalidOperationException($"Unknown unit: {unitPart}");
            var result = numPart * unitMul;
            if (floatPart > 0)
                result += unitMul.Multiply(floatPart);
            if (negative)
                result *= -1;
            return result;
        }

        public static string ToString(BigInteger originalValue, int floatPrecision = 1)
        {
            if (floatPrecision < 0)
                throw new ArgumentOutOfRangeException(nameof(floatPrecision));
            var value = originalValue;
            if (value == BigInteger.Zero)
                return "0";
            var negative = value.Sign < 0;
            if (negative)
                value = -value;
            var index = (int)BigInteger.Log(value, 1000);
            if (index >= Units.Count)
                throw new InvalidOperationException("The number is too big for converting to string.");
            if (index == 0)
                return originalValue.ToString();

            var floatMul = BigInteger.Pow(10, floatPrecision);
            var unit = Units[index];
            var mul = BigInteger.Pow(1000, index);
            var result = value * floatMul / mul;
            var primaryNum = result / floatMul;
            var floatNum = result % floatMul;
            while (floatNum > BigInteger.Zero && floatNum % 10 == BigInteger.Zero)
                floatNum /= 10;
            var numPart = floatNum == BigInteger.Zero ? primaryNum.ToString() : $"{primaryNum}.{Pad(floatNum, floatPrecision)}";
            return (negative ? "-" : string.Empty) + numPart + unit;
        }

        private static string Pad(BigInteger num, int paddedLength)
        {
            var s = num.ToString();
            var diff = Math.Max(0, paddedLength - s.Length);
            if (diff == 0)
                return s;
            return new string('0', diff) + s;
        }
    }
}