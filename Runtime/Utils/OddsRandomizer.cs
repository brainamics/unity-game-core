using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class OddsRandomizer<T>
    {
        private readonly List<OddsRecord> _odds = new();
        private double _maxOdds;

        public int OddsCount => _odds.Count;

        public bool IsValid => _maxOdds > 0;

        public void AddOdds(T value, double odds)
        {
            var end = _maxOdds + odds;
            _odds.Add(new OddsRecord
            {
                Value = value,
                Begin = _maxOdds,
                End = end,
            });
            _maxOdds = end;
        }

        public T Randomize()
        {
            var selected = Random.value * _maxOdds;
            foreach (var odds in _odds)
            {
                if (selected < odds.Begin || selected >= odds.End)
                    continue;
                return odds.Value;
            }
            throw new System.InvalidOperationException($"Unexpected algorithm issue. Odds record = {string.Join(',', _odds.Select(o => o.ToString()))}");
        }

        private struct OddsRecord
        {
            public double Begin;
            public double End;
            public T Value;

            public override readonly string ToString()
            {
                return $"{Begin}-{End}=>{Value}";
            }

            public override readonly int GetHashCode()
            {
                return System.HashCode.Combine(Begin, End, Value);
            }

            public override readonly bool Equals(object obj)
            {
                if (obj is OddsRecord rand)
                    return Begin == rand.Begin && End == rand.End && Equals(Value, rand.Value);
                return false;
            }
        }
    }
}
