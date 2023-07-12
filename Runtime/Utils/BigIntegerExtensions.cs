using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Brainamics.Core
{
    public static class BigIntegerExtensions
    {
        private const int BigIntegerPrecisionNumber = 1000;

        public static BigInteger Multiply(this BigInteger val, float mul)
        {
            if (mul == 1)
                return val;
            if (mul == 0)
                return BigInteger.Zero;
            mul *= BigIntegerPrecisionNumber;
            return val * (long)mul / BigIntegerPrecisionNumber;
        }
    
        public static BigInteger Multiply(this BigInteger val, double mul)
        {
            if (mul == 1)
                return val;
            if (mul == 0)
                return BigInteger.Zero;
            mul *= BigIntegerPrecisionNumber;
            return val * (long)mul / BigIntegerPrecisionNumber;
        }
    }
}
