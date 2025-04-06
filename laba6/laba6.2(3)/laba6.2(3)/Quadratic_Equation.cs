using Input_Validation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Quadratic_Equation 
{
    internal class QuadraticEquation
    {
        private double a;
        private double b;
        private double c;
        public double A
        {
            get { return a; }
            set { a = value; }
        }
        public double B
        {
            get { return b; }
            set { b = value; }
        }
        public double C
        {
            get { return c; }
            set { c = value; }
        }
        public QuadraticEquation(double a, double b, double c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
        public double[] CalculateRoots()
        {
            if (a == 0)
            {
                if (b == 0)
                {
                    return new double[0];
                }
                else
                {
                    if (c == 0)
                    {
                        return new double[] { 0 };
                    }
                    else
                    {
                        return new double[] { -c / b };
                    }
                }
            }

            double discriminant = b * b - 4 * a * c;

            if (discriminant < 0)
            {
                return new double[0];
            }
            else if (discriminant == 0)
            {
                if (c == 0)
                {
                    return new double[] { 0 };
                }
                else
                {
                    return new double[] { -b / (2 * a) };
                }
            }
            else
            {
                if (c == 0)
                {
                    return new double[] { 0, -b / a };
                }
                else
                {
                    double x1 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                    double x2 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                    return new double[] { x1, x2 };
                }
            }
        }

        public static QuadraticEquation operator ++(QuadraticEquation eq)
            {
                return new QuadraticEquation(eq.a + 1, eq.b + 1, eq.c + 1);
            }

            public static QuadraticEquation operator --(QuadraticEquation eq)
            {
                return new QuadraticEquation(eq.a - 1, eq.b - 1, eq.c - 1);
            }

            public static implicit operator double(QuadraticEquation eq)
            {
                if (eq.a == 0)
                {
                    return double.NaN; 
                }
                return eq.b * eq.b - 4 * eq.a * eq.c;
            }

            public static explicit operator bool(QuadraticEquation eq)
            {
                return eq.CalculateRoots().Length > 0;
            }

            public static bool operator ==(QuadraticEquation eq1, QuadraticEquation eq2)
            {
                if (ReferenceEquals(eq1, eq2)) return true;
                if (ReferenceEquals(eq1, null) || ReferenceEquals(eq2, null)) return false;

                return eq1.a == eq2.a && eq1.b == eq2.b && eq1.c == eq2.c;
            }

            public static bool operator !=(QuadraticEquation eq1, QuadraticEquation eq2)
            {
                return !(eq1 == eq2);
            }

            public override int GetHashCode()
            {
                return HashCode.Combine(a, b, c);
            }

            public override bool Equals(object? obj)
            {
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }

                QuadraticEquation other = (QuadraticEquation)obj;
                return a == other.a && b == other.b && c == other.c;
            }

            public override string ToString()
            {
                return $"a = {a}, b = {b}, c = {c}";
            }
        }
    }
