namespace Polynomials
{
    using System;
    using PolynomialLib;

    public class Program
    {
        public static void Main(string[] args)
        {
            // Capabilities of new string-polynom constructor.
            var poly1 = new Polynomial("1 + 2*x + 3*x^2 + 4*x^3 + 2 + 3*x + x^2");
            var poly2 = new Polynomial("x^3 - x^5");
            var poly3 = new Polynomial(6);

            poly3 = poly1 + poly2;

            Console.WriteLine(poly1);
            Console.WriteLine(poly2);
            Console.WriteLine(poly3);
            Console.WriteLine();
             
            // Initialization.
            Console.WriteLine("Initialization.\n");

            var polynom1 = new Polynomial(1, 2, 3, 4);

            double[] coefficients = { 2, 4, 6, 8 };
            var polynom2 = new Polynomial(coefficients);

            var polynom3 = new Polynomial(4);

            Console.WriteLine(polynom1.ToString());
            Console.WriteLine(polynom2.ToString());
            Console.WriteLine(polynom3.ToString());

            // Arithmetic operations.
            Console.WriteLine("\nArithmetic operations.\n");

            polynom3 = polynom1 + polynom2;
            Console.WriteLine(polynom3.ToString());

            polynom3 = polynom1 - polynom2;
            Console.WriteLine(polynom3.ToString());

            var constValue = 2;
            polynom3 = polynom1 * constValue;
            Console.WriteLine(polynom3.ToString());

            polynom3 = polynom1 * polynom2;
            Console.WriteLine(polynom3.ToString());

            // Finding a root on a segment.
            Console.WriteLine("\nFinding a root on a segment.\n");

            polynom1 = new Polynomial(-18, 0, 2);
            var root = Polynomial.GetRoot(polynom1, -10, 15);

            Console.WriteLine("root on [-10,15] is: {0}", root);

            // Getting an array of values from an array of arguments.
            Console.WriteLine("\nGetting an array of values from an array of arguments.\n");

            double[] x = { 1, 2, 3, 4 };
            double[] y;

            polynom1 = new Polynomial(1, 0, 1);
            y = Polynomial.Solve(polynom1, x);

            for (var i = 0; i < y.Length; i++)
            {
                Console.WriteLine("f(x[{0}]) = {1}", i, y[i]);
            }

            // Calculating the derivative.
            Console.WriteLine("\nCalculating the derivative.\n");

            polynom1 = new Polynomial(1, 2, 3, 4);
            Console.WriteLine("f(x) = " + polynom1.ToString());
            polynom1 = Polynomial.Differentiate(polynom1);
            Console.WriteLine("f'(x) = " + polynom1.ToString());

            // Calculating the integral.
            Console.WriteLine("\nCalculating the integral.\n");

            polynom1 = new Polynomial(1, 2, 3, 4);
            Console.WriteLine("f(x) = " + polynom1.ToString());
            polynom1 = Polynomial.Integrate(polynom1);
            Console.WriteLine("Integrf(x) = " + polynom1.ToString());

            Console.ReadKey();
        }
    }
}