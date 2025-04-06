using Number_Triplet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Input_Date 
{
    internal class Date : NumberTriplet
    {
        public Date(int day, int month, int year) : base(day, month, year) { }
        public Date(Date other) : base(other) { }
        public Date() : this(1, 1, 2000) { }

        public bool IsLeapYear()
        {
           return (Year % 4 == 0 && Year % 100 != 0) || Year % 400 == 0;
        }
        public bool IsValid()
        {
            if (Year < 1 || Month < 1 || Month > 12 || Day < 1) return false;
            int[] daysInMonth = { 0, 31, IsLeapYear() ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
            return Day <= daysInMonth[Month];
        }

        public string DayOfWeek()
        {
            try
            {
                var dateValue = new DateTime(Year, Month, Day);
                return dateValue.DayOfWeek.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                return "Недопустимая дата";  
            }
        }

        public int DaysSinceStartOfYear()
        {
            try
            {
                var dateValue = new DateTime(Year, Month, Day);
                return dateValue.DayOfYear;
            }
            catch (ArgumentOutOfRangeException)
            {
                return -1; 
            }
        }

        public override string ToString()
        {
            return $"Дата: {Day:D2}.{Month:D2}.{Year}";
        }
    }
}
