namespace Calculator
{
    using System;
    using Exceptions;

    public static class Program
    {
        public static void Main()
        {
            var calculator = new Calculator();
            var parser = new Parser();

            try
            {
                var sqrt = new Func<double, double>(
                    x =>
                    {
                        if (x < 0)
                        {
                            throw new ArgumentOutOfRangeException();
                        }

                        return Math.Sqrt(x);
                    });

                calculator.DefineOperation("sqrt", sqrt);
                calculator.DefineOperation("-", a => -a);
                calculator.DefineOperation("-", (a, b) => a - b);
                calculator.DefineOperation("^", Math.Pow);
            }
            catch (AlreadyExistsOperationException)
            {
                Console.WriteLine("This operation already esixst in the calculator");
            }

            var evaluator = new Evaluator(calculator, parser);
            Console.WriteLine("Please enter expressions: ");

            while (true)
            {
                string line = Console.ReadLine();
                if (line == null || line.Trim().Length == 0)
                {
                    break;
                }

                try
                {
                    Console.WriteLine(evaluator.Calculate(line));
                }
                catch (AlreadyExistsOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (IncorrectParametersException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (NotFoundOperationException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (UnknownOperationTypeException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}