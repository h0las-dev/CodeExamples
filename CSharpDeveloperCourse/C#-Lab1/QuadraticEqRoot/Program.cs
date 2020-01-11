namespace QuadraticEqRoot
{
    using System;
    using System.Text;

    public class Program
    {
        // Set const precision for comparison with zero.
        private const double Eps = 1E-9;

        public static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Please set the quadratic equation");

                Console.Write("a: ");
                double a = Convert.ToDouble(Console.ReadLine());
                if (Math.Abs(a) < Eps)
                {
                    Console.WriteLine("The coefficient <a> is zero or close to zero. We take a = 0.");
                    a = 0;
                }
            
                Console.Write("b: ");
                double b = Convert.ToDouble(Console.ReadLine());
                if (Math.Abs(b) < Eps)
                {
                    Console.WriteLine("The coefficient <b> is zero or close to zero. We take b = 0.");
                    b = 0;
                }

                Console.Write("c: ");
                double c = Convert.ToDouble(Console.ReadLine());
                if (Math.Abs(c) < Eps)
                {
                    Console.WriteLine("The coefficient <c> is zero or close to zero. We take c = 0.");
                    c = 0;
                }

                if (a != 0)
                {
                    double discriminant = (b * b) - (4 * a * c);
                    if (discriminant > 0)
                    {
                        double x1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                        double x2 = (-b + Math.Sqrt(discriminant)) / (2 * a);

                        Console.Write("x1 = {0}, x2 = {1}\n", x1, x2);
                    }
                    else if (Math.Abs(discriminant) < Eps)
                    {
                        double x = -b / (2 * a);

                        Console.Write("x = {0}\n", x);
                    }
                    else
                    {
                        double re = -b / (2 * a);
                        double im = Math.Sqrt(Math.Abs(discriminant)) / (2 * a);

                        if (Math.Abs(re) < 0)
                        {
                            Console.Write("x1 = {0}i, x2 = {1}i\n", im, -im);
                        }
                        else
                        {
                            if (im > 0)
                            {
                                Console.Write("x1 = {0} + {1}i, x2 = {0} - {2}i\n", re, im, im);
                            }
                            else
                            {
                                Console.Write("x1 = {0} + {1}i, x2 = {0} - {2}i\n", re, -im, -im);
                            }
                        }
                    }
                }
                else
                {
                    if (b == 0)
                    {
                        if (c == 0)
                        {
                            Console.Write("solution = infinity\n");
                        }
                        else
                        {
                            Console.Write("no solutions\n");
                        }
                    }
                    else
                    {
                        double x = -c / b;
                        Console.Write("x = {0}\n", x);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            Console.ReadKey();
        }
    }
}