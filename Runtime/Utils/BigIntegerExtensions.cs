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

        public static BigInteger Multiply(this BigInteger val, float mul, int precisionNumber = BigIntegerPrecisionNumber)
        {
            mul *= precisionNumber;
            return val * (long)mul / precisionNumber;
        }

        public static BigInteger Multiply(this BigInteger val, double mul, int precisionNumber = BigIntegerPrecisionNumber)
        {
            mul *= precisionNumber;
            return val * (long)mul / precisionNumber;
        }
    }
}