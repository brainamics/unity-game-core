using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public sealed class OddsRandomizer<T>
    {
        private readonly List<Odds> _odds = new();
        private double _maxOdds;

        public void AddOdds(T value, double odds)
        {
            var end = _maxOdds + odds;
            _odds.Add(new Odds
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
            throw new System.InvalidOperationException("Unexpected algorithm issue.");
        }

        private struct Odds
        {
            public double Begin;
            public double End;
            public T Value;
        }
    }
}