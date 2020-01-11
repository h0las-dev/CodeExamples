namespace Calculator
{
    using System.Text.RegularExpressions;
    using Exceptions;

    public class Parser
    {
        public Operation Parse(string inputString)
        {
            var stringAfterParse = Regex.Replace(inputString, @"\s+", " ").Trim();
            var symbolsArray = stringAfterParse.Split(' ');

            var tmpSignValue = new double();
            var operation = new Operation();

            var operationError = false;

            if (symbolsArray.Length < 2)
            {
                throw new UnknownOperationTypeException();
            }

            if (double.TryParse(symbolsArray[0], out tmpSignValue))
            {
                operationError = true;
            }

            var argumentsArray = new double[symbolsArray.Length - 1];

            for (var i = 0; i < argumentsArray.Length; i++)
            {
                if (double.TryParse(symbolsArray[i + 1], out tmpSignValue))
                {
                    argumentsArray[i] = tmpSignValue;
                }
                else
                {
                    throw new IncorrectParametersException();
                }
            }

            if (operationError)
            {
                throw new UnknownOperationTypeException();
            }

            operation.Sign = symbolsArray[0];
            operation.Parameters = argumentsArray;

            return operation;
        }
    }
}