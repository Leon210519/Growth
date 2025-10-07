using System;
using System.Globalization;
using UnityEngine;

namespace Game.Core.Economy.Balancing
{
    /// <summary>
    /// Represents large values with helper methods for idle game scale numbers.
    /// Stores values internally as double while providing utility for formatting and safe arithmetic.
    /// </summary>
    [Serializable]
    public struct BigNumber : IComparable<BigNumber>, IEquatable<BigNumber>
    {
        private const double Epsilon = 1e-9;
        [SerializeField] private double _value;

        public BigNumber(double value)
        {
            _value = value;
        }

        public double RawValue => _value;

        public static implicit operator BigNumber(double value) => new BigNumber(value);
        public static implicit operator double(BigNumber value) => value._value;

        public static BigNumber operator +(BigNumber a, BigNumber b) => new BigNumber(a._value + b._value);
        public static BigNumber operator -(BigNumber a, BigNumber b) => new BigNumber(a._value - b._value);
        public static BigNumber operator *(BigNumber a, BigNumber b) => new BigNumber(a._value * b._value);
        public static BigNumber operator /(BigNumber a, BigNumber b) => new BigNumber(a._value / b._value);

        public static bool operator >(BigNumber a, BigNumber b) => a._value > b._value + Epsilon;
        public static bool operator <(BigNumber a, BigNumber b) => a._value + Epsilon < b._value;
        public static bool operator >=(BigNumber a, BigNumber b) => a._value >= b._value - Epsilon;
        public static bool operator <=(BigNumber a, BigNumber b) => a._value <= b._value + Epsilon;

        public static BigNumber Max(BigNumber a, BigNumber b) => a >= b ? a : b;
        public static BigNumber Min(BigNumber a, BigNumber b) => a <= b ? a : b;

        public string ToAbbreviatedString(int decimals = 1)
        {
            return Format(_value, decimals);
        }

        public override string ToString()
        {
            return ToAbbreviatedString(2);
        }

        public static string Format(double value, int decimals)
        {
            if (double.IsNaN(value) || double.IsInfinity(value))
            {
                return "0";
            }

            var abs = Math.Abs(value);
            if (abs < 1000d)
            {
                return Math.Round(value, decimals).ToString("N" + decimals, CultureInfo.InvariantCulture);
            }

            string[] suffixes =
            {
                "K", "M", "B", "T", "aa", "ab", "ac", "ad", "ae", "af"
            };

            int index = -1;
            while (abs >= 1000d && index < suffixes.Length - 1)
            {
                abs /= 1000d;
                index++;
            }

            double shortValue = Math.Sign(value) * abs;
            return string.Format(CultureInfo.InvariantCulture, "{0:F" + Mathf.Clamp(decimals, 0, 3) + "}{1}", shortValue, suffixes[Math.Max(index, 0)]);
        }

        public int CompareTo(BigNumber other)
        {
            return _value.CompareTo(other._value);
        }

        public bool Equals(BigNumber other)
        {
            return Math.Abs(_value - other._value) < Epsilon;
        }

        public override bool Equals(object obj)
        {
            return obj is BigNumber number && Equals(number);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
