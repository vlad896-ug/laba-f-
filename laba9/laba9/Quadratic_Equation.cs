
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
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException("Коэффициент 'a' должен быть конечным числом.", nameof(value));
                }
                a = value;
            }
        }

        public double B
        {
            get { return b; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException("Коэффициент 'b' должен быть конечным числом.", nameof(value));
                }
                b = value;
            }
        }

        public double C
        {
            get { return c; }
            set
            {
                if (double.IsNaN(value) || double.IsInfinity(value))
                {
                    throw new ArgumentException("Коэффициент 'c' должен быть конечным числом.", nameof(value));
                }
                c = value;
            }
        }

        public QuadraticEquation(double a, double b, double c)
        {
            A = a;
            B = b;
            C = c;
        }

        public double[] CalculateRoots()
        {
            if (A == 0)
            {
                if (B == 0)
                {
                    return new double[0];
                }
                else
                {
                    if (C == 0)
                    {
                        return new double[] { 0 };
                    }
                    else
                    {
                        return new double[] { -C / B };
                    }
                }
            }

            double discriminant = B * B - 4 * A * C;

            if (discriminant < 0)
            {
                return new double[0];
            }
            else if (discriminant == 0)
            {
                if (C == 0)
                {
                    return new double[] { 0 };
                }
                else
                {
                    return new double[] { -B / (2 * A) };
                }
            }
            else
            {
                if (C == 0)
                {
                    return new double[] { 0, -B / A };
                }
                else
                {
                    double x1 = (-B + Math.Sqrt(discriminant)) / (2 * A);
                    double x2 = (-B - Math.Sqrt(discriminant)) / (2 * A);
                    return new double[] { x1, x2 };
                }
            }
        }

        public static QuadraticEquation operator ++(QuadraticEquation equation)
        {
            return new QuadraticEquation(equation.A + 1, equation.B + 1, equation.C + 1);
        }

        public static QuadraticEquation operator --(QuadraticEquation equation)
        {
            return new QuadraticEquation(equation.A - 1, equation.B - 1, equation.C - 1);
        }

        public static implicit operator double(QuadraticEquation equation)
        {
            if (equation.A == 0)
            {
                return double.NaN;
            }
            return equation.B * equation.B - 4 * equation.A * equation.C;
        }

        public static explicit operator bool(QuadraticEquation equation)
        {
            return equation.CalculateRoots().Length > 0;
        }

        public static bool operator ==(QuadraticEquation equation1, QuadraticEquation equation2)
        {
            if (ReferenceEquals(equation1, equation2))
            {
                return true;
            }

            if (equation1 is null || equation2 is null)
            {
                return false;
            }
            return equation1.A == equation2.A && equation1.B == equation2.B && equation1.C == equation2.C;
        }

        public static bool operator !=(QuadraticEquation equation1, QuadraticEquation equation2)
        {
            return !(equation1 == equation2);
        }

        public override string ToString()
        {
            return $"a = {A}, b = {B}, c = {C}";
        }
        }
    }
