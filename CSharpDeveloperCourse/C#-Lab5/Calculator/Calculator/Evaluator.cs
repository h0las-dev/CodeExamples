namespace Calculator
{
    public class Evaluator
    {
        private readonly Calculator calculator;

        private readonly Parser parser;

        public Evaluator(Calculator calculator, Parser parser)
        {
            this.calculator = calculator;
            this.parser = parser;
        }

        public string Calculate(string inputString)
        {
            {
                var operation = new Operation();

                operation = this.parser.Parse(inputString);

                var operationValue = this.calculator.PerformOperation(operation);

                return operationValue.ToString();
            }
        }
    }
}