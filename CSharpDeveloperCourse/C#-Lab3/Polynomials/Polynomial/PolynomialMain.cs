namespace PolynomialLib
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Polynomial class
    /// </summary>
    public class Polynomial
    {
        //----- constants ----------------------------------------------------------

        // If some value < eps then value is zero
        private const double Eps = 1E-15;

        //----- fields -------------------------------------------------------------
        private readonly List<double> polynomialCoefficients;

        //----- public ------------------------------------------------------------- 

        /// <summary>
        /// Create a polynomial of a given degree
        /// </summary>
        /// <param name="degree">degree of polynomial</param>
        public Polynomial(int degree)
        {
            this.polynomialCoefficients = new List<double>(degree);

            for (var i = 0; i < degree; i++)
            {
                this.polynomialCoefficients.Add(0);
            }
        }

        /// <summary>
        /// Create polynomial by a string
        /// </summary>
        /// <param name="polynom">string of polynomial</param>
        public Polynomial(string polynom)
        {
            this.polynomialCoefficients = new List<double>();

            polynom = Regex.Replace(polynom, "\\s+", string.Empty);

            // I Calculate the degree of a polynimial
            var regexDegree = new Regex(@"x\^\d+");
            var matchDegree = regexDegree.Match(polynom);
            var degreeString = string.Empty;
            var degreeInt = 0;
            var maxDegree = 0;
            while (matchDegree.Success)
            {
                degreeString = matchDegree.Value;
                degreeString = degreeString.Replace("x^", string.Empty);
                if (int.TryParse(degreeString, out degreeInt))
                {
                    if (degreeInt > maxDegree)
                    {
                        maxDegree = degreeInt;
                    }
                }
                else
                {
                    throw new ArgumentException();
                }

                matchDegree = matchDegree.NextMatch();
            }

            maxDegree += 2;

            for (var i = 0; i < maxDegree; i++)
            {
                this.polynomialCoefficients.Add(0);
            }

            var regex = new Regex(@"([+-]?\d*\*?x\^\d+)|([+-]?\d*\*?x)|([+-]?\d+)");
            var match = regex.Match(polynom);
            while (match.Success)
            {
                var monomial = match.Value;
                if (monomial.Contains("x"))
                {
                    if (monomial.Contains("^"))
                    {
                        monomial = monomial.Replace("^", string.Empty);
                        monomial = monomial.Replace("*", string.Empty);

                        // I will split this string. value[0] = coefficient, value[1] = index in polynominalCoefficients.
                        var coeffStringArray = monomial.Split('x');

                        var number = 0.0;
                        var index = 0;

                        if (coeffStringArray[0] == "+" || monomial[0] == 'x')
                        {
                            number = 1;
                            if (int.TryParse(coeffStringArray[1], out index))
                            {
                                this[index] += number;
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                        else if (coeffStringArray[0] == "-")
                        {
                            number = -1;
                            if (int.TryParse(coeffStringArray[1], out index))
                            {
                                this[index] += number;
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                        else
                        {
                            if (double.TryParse(coeffStringArray[0], out number) && int.TryParse(coeffStringArray[1], out index))
                            {
                                this[index] += number;
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                    }
                    else
                    {
                        monomial = monomial.Replace("x", string.Empty);
                        monomial = monomial.Replace("*", string.Empty);
                        var number = 0.0;
                        var index = 1; 

                        if (monomial == "+" || monomial == string.Empty)
                        {
                            number = 1;
                            this[index] += number;
                        }
                        else if (monomial == "-")
                        {
                            number = -1;
                            this[index] += number;
                        }
                        else
                        {
                            if (double.TryParse(monomial, out number))
                            {
                                this[index] += number;
                            }
                            else
                            {
                                throw new ArgumentException();
                            }
                        }
                    }
                }
                else
                {
                    var number = 0.0;
                    var index = 0;
                    if (double.TryParse(monomial, out number))
                    {
                        this[index] += number;
                    }
                    else
                    {
                        throw new ArgumentException();
                    }
                }

                match = match.NextMatch();
            }

            for (int i = 0; i < this.polynomialCoefficients.Count; i++)
            {
                Console.WriteLine("{0}, ", this.polynomialCoefficients[i]);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Create a polynomial by params
        /// </summary>
        /// <param name="coefficients">coefficients of polynomial</param>
        public Polynomial(params double[] coefficients)
        {
            if (coefficients == null || coefficients.Length < 1)
            {
                this.polynomialCoefficients = new List<double> { 0 };
            }
            else
            {
                this.polynomialCoefficients = new List<double>();

                for (var i = 0; i < coefficients.Length; i++)
                {
                    this.polynomialCoefficients.Add(coefficients[i]);
                }
            }
        }

        /// <summary>
        /// Create a polynomial by array
        /// </summary>
        /// <param name="coeffArray">array of coefficients</param>
        public Polynomial(IEnumerable<double> coeffArray)
        {
            this.polynomialCoefficients = new List<double>(coeffArray);
        }

        /// <summary>
        /// Get coefficients
        /// </summary>
        public IEnumerable<double> GetCoeffArray => this.polynomialCoefficients;

        /// <summary>
        /// Get Degree
        /// </summary>
        public int Degree => this.polynomialCoefficients.Count;

        /// <summary>
        /// Access by index
        /// </summary>
        /// <param name="i">index of coefficient</param>
        /// <returns>coefficient[i]</returns>
        public double this[int i]
        {
            get
            {
                if (i < 0 || i >= this.polynomialCoefficients.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                return this.polynomialCoefficients[i];
            }

            set
            {
                if (i < 0 || i >= this.polynomialCoefficients.Count)
                {
                    throw new IndexOutOfRangeException();
                }

                this.polynomialCoefficients[i] = value;
            }
        }

        /// <summary>
        /// Overload Operation Addition
        /// </summary>
        /// <param name="polynom1">polynimial1</param>
        /// <param name="polynom2">polynomial2</param>
        /// <returns>polynomial1 + polynomial2</returns>
        public static Polynomial operator +(Polynomial polynom1, Polynomial polynom2)
        {
            var maxPolynom = GetMaxPolymom(polynom1, polynom2);
            var minPolynom = GetMinPolynom(polynom1, polynom2);

            var resultPolynom = new Polynomial(maxPolynom.polynomialCoefficients);

            for (var i = 0; i < minPolynom.Degree; i++)
            {
                resultPolynom[i] += minPolynom[i];
            }

            return resultPolynom;
        }

        /// <summary>
        /// Overload Operation Subtraction
        /// </summary>
        /// <param name="polynom1">polynimial1</param>
        /// <param name="polynom2">polynomial2</param>
        /// <returns>polynomial1 - polynomial2</returns>
        public static Polynomial operator -(Polynomial polynom1, Polynomial polynom2)
        {
            var resultPolynom = polynom1 + (polynom2 * -1);

            return resultPolynom;
        }

        /// <summary>
        /// Overload Operation Multiplication
        /// </summary>
        /// <param name="polynom1">polynimial</param>
        /// <param name="constValue">constant value</param>
        /// <returns>polynomial * constant value</returns>
        public static Polynomial operator *(Polynomial polynom1, double constValue)
        {
            var resultPoly = new Polynomial(polynom1.GetCoeffArray);

            for (int i = 0; i < polynom1.Degree; i++)
            {
                resultPoly[i] *= constValue;
            }

            return resultPoly;
        }

        /// <summary>
        /// Overload Operation Multiplication
        /// </summary>
        /// <param name="polynom1">polynimial1</param>
        /// <param name="polynom2">polynimial2</param>
        /// <returns>polynomial * polynimial2</returns>
        public static Polynomial operator *(Polynomial polynom1, Polynomial polynom2)
        {
            var resultPolynom = new Polynomial(polynom1.Degree + polynom2.Degree - 1);

            for (var i = 0; i < resultPolynom.Degree; i++)
            {
                resultPolynom[i] = 0;
            }

            for (var i = 0; i < polynom1.Degree; i++)
            {
                for (var j = 0; j < polynom2.Degree; j++)
                {
                    resultPolynom[i + j] += polynom1[i] * polynom2[j];
                }
            }

            return resultPolynom;
        }

        /// <summary>
        /// Get the derivative
        /// </summary>
        /// <param name="polynom">polynomial</param>
        /// <returns>d(polynomial)</returns>
        public static Polynomial Differentiate(Polynomial polynom)
        {
            var resultPolynom = new Polynomial(polynom.Degree - 1);

            for (var i = 1; i < polynom.Degree; i++)
            {
                resultPolynom[i - 1] = i * polynom[i];
            }

            return resultPolynom;
        }

        /// <summary>
        /// Get the Integral
        /// </summary>
        /// <param name="polynom">polynomial</param>
        /// <returns>Integr(polynomial)</returns>
        public static Polynomial Integrate(Polynomial polynom)
        {
            var resultPolynom = new Polynomial(polynom.Degree + 1) { [0] = 0 };

            for (var i = 1; i < resultPolynom.Degree; i++)
            {
                resultPolynom[i] = Math.Round(polynom[i - 1] / i, 4);
            }

            return resultPolynom;
        }

        /// <summary>
        /// Get an array of values from an array of arguments
        /// </summary>
        /// <param name="polynom">polynomial</param>
        /// <param name="x">array of arguments</param>
        /// <returns>array of values</returns>
        public static double[] Solve(Polynomial polynom, double[] x)
        {
            var result = new double[x.Length];

            for (var i = 0; i < x.Length; i++)
            {
                result[i] = polynom.Solve(x[i]);
            }

            return result;
        }

        /// <summary>
        /// Get the root on a segment
        /// </summary>
        /// <param name="polynom">polynomial</param>
        /// <param name="a">left border</param>
        /// <param name="b">right border</param>
        /// <returns>root</returns>
        public static double GetRoot(Polynomial polynom, double a, double b)
        {
            if (Math.Abs(polynom.Solve(a)) < Eps)
            {
                return a;
            }

            if (Math.Abs(polynom.Solve(b)) < Eps)
            {
                return b;
            }

            while (Math.Abs(b - a) > Eps)
            {
                var c = (a + b) / 2;

                if (Math.Abs(polynom.Solve(c)) < Eps)
                {
                    return c;
                }

                if (polynom.Solve(b) * polynom.Solve(c) < 0)
                {
                    a = c;
                }
                else
                {
                    b = c;
                }
            }

            if (Math.Abs(polynom.Solve((a + b) / 2)) < Eps)
            {
                return (a + b) / 2;
            }

            throw new ArgumentException("There are no roots on this segment!");
        }

        /// <summary>
        /// Convert a polynomial to a string
        /// </summary>
        /// <returns>polynomial(a0 + a1*x + a2*x^2 + ... an*x^n)</returns>
        public override string ToString()
        {
            StringBuilder polynomString = new StringBuilder();

            if (!this.IsZero())
            {
                for (var i = 0; i < this.Degree; i++)
                {
                    if (Math.Abs(this.polynomialCoefficients[i]) > Eps)
                    {
                        if (i == 0)
                        {
                            polynomString.Append(this.polynomialCoefficients[i]);
                        }
                        else if (i == 1)
                        {
                            polynomString.Append(this.polynomialCoefficients[i] + "x");
                        }
                        else
                        {
                            if (this.polynomialCoefficients[i] > 0)
                            {
                                polynomString.Append(this.polynomialCoefficients[i] + "x^" + i);
                            }
                            else
                            {
                                polynomString.Append("(" + this.polynomialCoefficients[i] + ")" + "x^" + i);
                            }
                        }
                    }

                    if (i < this.Degree - 1)
                    {
                        if ((Math.Abs(this.polynomialCoefficients[i + 1]) > 0) && (polynomString.Length > 0))
                        {
                            polynomString.Append(" + ");
                        }
                    }
                }

                return polynomString.ToString();
            }

            return "0";
        }

        //----- private ------------------------------------------------------------
        private static Polynomial GetMaxPolymom(Polynomial polynom1, Polynomial polynom2)
        {
            if (polynom1.Degree >= polynom2.Degree)
            {
                return polynom1;
            }

            return polynom2;
        }

        private static Polynomial GetMinPolynom(Polynomial polynom1, Polynomial polynom2)
        {
            if (polynom1.Degree < polynom2.Degree)
            {
                return polynom1;
            }

            return polynom2;
        }

        private double Solve(double x)
        {
            var resault = 0.0;

            for (var i = 0; i < this.Degree; i++)
            {
                resault += this[i] * Math.Pow(x, i);
            }

            return resault;
        }

        private bool IsZero()
        {
            for (var i = 0; i < this.Degree; i++)
            {
                if (Math.Abs(this.polynomialCoefficients[i]) > Eps)
                {
                    return false;
                }
            }

            return true;
        }
    }
}