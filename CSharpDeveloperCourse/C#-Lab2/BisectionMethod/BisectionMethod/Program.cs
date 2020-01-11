namespace BisectionMethod
{
    using System;

    public class Program
    {
        private const double Eps = 1E-4;

        public static double Function(double x)
        {
            return (x * x) - 2;
        }

        public static void Main()
        {
            var a = 0.0;
            var b = 2.0;
            var iterCount = 0;

            while (Math.Abs(b - a) > Eps)
            {
                var c = (a + b) / 2;

                if (Function(b) * Function(c) < 0)
                {
                    a = c;
                }
                else
                {
                    b = c;
                }

                iterCount++;
            }

            Console.WriteLine("Root of the equation is: {0}", (a + b) / 2);
            Console.WriteLine("Amount of iterations is: {0}", iterCount);

            Console.ReadKey();
        }
    }
}
