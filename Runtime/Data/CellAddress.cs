using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Brainamics.Core
{
    [Serializable]
    public struct CellAddress
    {
        public static readonly CellAddress Invalid = new(-1, -1);
        public static readonly CellAddress Zero = new(0, 0);
        public static readonly CellAddress One = new(1, 1);
        public static readonly CellAddress MinValue = new(int.MinValue, int.MinValue);
        public static readonly CellAddress MaxValue = new(int.MaxValue, int.MaxValue);

        public int Row;
        public int Column;

        public bool IsValid => Row >= 0 && Column >= 0;

        public CellAddress(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public static IEnumerable<CellAddress> Enumerate(CellAddress from, CellAddress to)
        {
            for (var row = from.Row; row <= to.Row; row++)
            {
                for (var column = from.Column; column <= to.Column; column++)
                {
                    yield return new CellAddress(row, column);
                }
            }
        }

        public readonly CellAddress MoveStep(Vector2Int amount, bool upDeducts = true)
        {
            if (upDeducts)
                amount.y *= -1;
            return new CellAddress(Row + amount.y, Column + amount.x);
        }

        /// <summary>
        /// Moves the address in a primary or diagonal direction.
        /// </summary>
        public readonly CellAddress MoveStep(PrimaryDirection direction, bool upDeducts = true)
        {
            var addr = this;
            if (direction == PrimaryDirection.None)
                return addr;
            foreach (var dir in direction.ValidateAndDeconstruct())
                addr = addr.MovePrimaryStep(dir, upDeducts);
            return addr;
        }

        public readonly CellAddress MovePrimaryStep(PrimaryDirection direction, bool upDeducts = true)
        {
            var upAddition = upDeducts ? -1 : 1;
            return direction switch
            {
                PrimaryDirection.Up => new CellAddress(Row + upAddition, Column),
                PrimaryDirection.Down => new CellAddress(Row - upAddition, Column),
                PrimaryDirection.Left => new CellAddress(Row, Column - 1),
                PrimaryDirection.Right => new CellAddress(Row, Column + 1),
                _ => throw new NotImplementedException($"Moving one step towards {direction} is not implemented."),
            };
        }

        public readonly CellAddress GetNearestInvalidAddress(int rows, int columns)
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

        public static bool TryParse(string str, out CellAddress address)
        {
            address = Invalid;
            if (string.IsNullOrEmpty(str))
                return false;

            var parts = str.Split(',');
            if (parts.Length != 2)
                return false;

            if (int.TryParse(parts[0], out var row) && int.TryParse(parts[1], out var column))
            {
                address = new CellAddress(row, column);
                return true;
            }
            return false;
        }

        public static CellAddress TryParse(string str)
        {
            return TryParse(str, out var address) ? address : Invalid;
        }

        public static CellAddress Parse(string str)
        {
            if (TryParse(str, out var address))
                return address;
            throw new FormatException($"The string '{str}' is not a valid cell address.");
        }

        public override readonly string ToString()
        {
            return $"{Row},{Column}";
        }

        public override readonly bool Equals(object obj)
        {
            if (obj is CellAddress addr)
                return Row == addr.Row && Column == addr.Column;
            return false;
        }

        public override readonly int GetHashCode()
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

        public static CellAddress operator +(CellAddress a, CellAddress b)
            => new(a.Row + b.Row, a.Column + b.Column);

        public static CellAddress operator -(CellAddress a, CellAddress b)
            => new(a.Row - b.Row, a.Column - b.Column);

        public static implicit operator Vector2Int(CellAddress addr)
            => new(addr.Column, addr.Row);

        public static implicit operator CellAddress(Vector2Int addr)
            => new(addr.y, addr.x);

        public static implicit operator ValueTuple<int, int>(CellAddress addr)
            => new(addr.Row, addr.Column);

        public static implicit operator CellAddress(ValueTuple<int, int> addr)
            => new(addr.Item1, addr.Item2);
    }

    public static class CellAddressExtensions
    {
        public static CellAddress WithRow(this CellAddress address, int row)
        {
            return new CellAddress(row, address.Column);
        }

        public static CellAddress WithColumn(this CellAddress address, int column)
        {
            return new CellAddress(address.Row, column);
        }
        
        public static bool IsValid(this CellAddress address, int rows, int columns)
        {
            if (!address.IsValid)
                return false;
            return address.Row < rows && address.Column < columns;
        }

        public static IEnumerable<CellAddress> GetAdjacentAddresses(this CellAddress address, int rows, int columns, bool primary = true, bool diagonal = false)
        {
            foreach (var dir in PrimaryDirectionUtils.EnumerateDirections(primary, diagonal))
            {
                var addr = address.MoveStep(dir);
                if (addr.IsValid(rows, columns))
                    yield return addr;
            }
        }

        public static void ThrowIfInvalid(this CellAddress address)
        {
            if (!address.IsValid)
                throw new System.InvalidOperationException("Invalid cell address.");
        }
    }
}
