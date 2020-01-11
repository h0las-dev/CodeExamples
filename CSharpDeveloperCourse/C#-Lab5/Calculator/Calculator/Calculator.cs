namespace Calculator
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class Calculator
    {
        private Dictionary<string, Func<double, double>> unaryFunctions = new Dictionary<string, Func<double, double>>();
        private Dictionary<string, Func<double, double, double>> binaryFunctions = new Dictionary<string, Func<double, double, double>>();
        private Dictionary<string, Func<double, double, double, double>> ternaryFunctions = new Dictionary<string, Func<double, double, double, double>>();

        public Calculator()
        {
            this.ternaryFunctions.Add("T5", (x, y, z) => z > 5 ? x : y);
            this.unaryFunctions.Add("++", x => x + 1);
            this.binaryFunctions.Add("*", (x, y) => x * y);
        }

        public double PerformOperation(Operation operation)
        {
            if (operation.Parameters.Length == 2)
            {
                if (this.binaryFunctions.ContainsKey(operation.Sign))
                {
                    return this.binaryFunctions[operation.Sign](operation.Parameters[0], operation.Parameters[1]);
                }
                else
                {
                    throw new NotFoundOperationException();
                }
            }
            else if (operation.Parameters.Length == 3)
            {
                if (this.ternaryFunctions.ContainsKey(operation.Sign))
                {
                    return this.ternaryFunctions[operation.Sign](operation.Parameters[0], operation.Parameters[1], operation.Parameters[2]);
                }
                else
                {
                    throw new NotFoundOperationException();
                }
            }
            else if (operation.Parameters.Length == 1)
            {
                if (this.unaryFunctions.ContainsKey(operation.Sign))
                {
                    return this.unaryFunctions[operation.Sign](operation.Parameters[0]);
                }
                else
                {
                    throw new NotFoundOperationException();
                }
            }
            else
            {
                throw new IncorrectParametersException();
            }
        }

        public void DefineOperation(string sign, Func<double, double, double, double> body)
        {
            if (this.ternaryFunctions.ContainsKey(sign))
            {
                throw new AlreadyExistsOperationException();
            }
            else
            {
                this.ternaryFunctions.Add(sign, body);
            }
        }

        public void DefineOperation(string sign, Func<double, double, double> body)
        {
            if (this.binaryFunctions.ContainsKey(sign))
            {
                throw new AlreadyExistsOperationException();
            }
            else
            {
                this.binaryFunctions.Add(sign, body);
            }
        }

        public void DefineOperation(string sign, Func<double, double> body)
        {
            if (this.unaryFunctions.ContainsKey(sign))
            {
                throw new AlreadyExistsOperationException();
            }
            else
            {
                this.unaryFunctions.Add(sign, body);
            }
        }
    }
}