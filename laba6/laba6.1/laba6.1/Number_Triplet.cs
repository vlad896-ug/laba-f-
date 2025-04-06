using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Number_Triplet
{
    internal class NumberTriplet
    {
        public int Day { get; protected set; }
        public int Month { get; protected set; }
        public int Year { get; protected set; }

        public NumberTriplet(int number1, int number2, int number3)
        {
            Day = number1;
            Month = number2;
            Year = number3;
        }

        public NumberTriplet(NumberTriplet other)
        {
            Day = other.Day;
            Month = other.Month;
            Year = other.Year;
        }

        public int MinLastDigit()
        {
            int lastDigit1 = Math.Abs(Day % 10);
            int lastDigit2 = Math.Abs(Month % 10);
            int lastDigit3 = Math.Abs(Year % 10);

            return Math.Min(lastDigit1, Math.Min(lastDigit2, lastDigit3));
        }

        public override string ToString()
        {
            return $"Число 1: {Day}, Число 2: {Month}, Число 3: {Year}";
        }
    }
}
