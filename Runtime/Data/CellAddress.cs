using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    public readonly struct CellAddress
    {
        public static readonly CellAddress Invalid = new(-1, -1);
    
        public readonly int Row;
        public readonly int Column;
    
        public bool IsValid => Row >= 0 && Column >= 0;
    
        public CellAddress(int row, int column)
        {
            Row = row;
            Column = column;
        }
            
        public CellAddress MoveStep(Vector2Int amount)
        {
            return new CellAddress(Row - amount.y, Column + amount.x);
        }
            
        public CellAddress GetNearestInvalidAddress(int rows, int columns)
        {
            var row = Row;
            var column = Column;
            if (!IsValid)
                return this;
    
            var distances = new ValueTuple<int, Action>[] {
                (Row, () => row = -1),
                (Column, () => column = -1),
                (rows - Row, () => row = rows),
                (columns - Column, () => column = columns),
            };
            var min = distances[0];
            foreach (var distance in distances)
                if (distance.Item1 < min.Item1)
                    min = distance;
            min.Item2();
    
            return new CellAddress(row, column);
        }
    
        public override string ToString()
        {
            return $"{Row},{Column}";
        }
    
        public override bool Equals(object obj)
        {
            if (obj is CellAddress addr)
                return Row == addr.Row && Column == addr.Column;
            return false;
        }
    
        public override int GetHashCode()
        {
            return HashCode.Combine(Row, Column);
        }
    
        public static bool operator ==(CellAddress a, CellAddress b)
        {
            return a.Equals(b);
        }
    
        public static bool operator !=(CellAddress a, CellAddress b)
        {
            return !a.Equals(b);
        }

        public static implicit Vector2Int(CellAddress addr)
            => new(addr.Column, addr.Row);

        public static implicit CellAddress(Vector2Int addr)
            => new(addr.y, addr.x);
    }
}
