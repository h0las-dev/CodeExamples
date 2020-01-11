namespace UnitTestCalculator
{
    using System;

    using Calculator;
    using Calculator.Exceptions;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CalculatorTest
    {
        [TestMethod]
        public void PerformOperationTest()
        {
            var operation = new Operation { Sign = "++", Parameters = new[] { 3.0 } };
            var calculator = new Calculator();

            var expected = 4.0;
            var actual = calculator.PerformOperation(operation);
            Assert.AreEqual(expected, actual, 0.001, "Incorrect calculation result");

            operation = new Operation { Sign = "*", Parameters = new[] { 5.0, 3.0 } };
            expected = 15.0;
            actual = calculator.PerformOperation(operation);
            Assert.AreEqual(expected, actual, 0.001, "Incorrect calculation result");

            operation = new Operation { Sign = "T5", Parameters = new[] { 2.0, 3.0, 6.0 } };
            expected = 2.0;
            actual = calculator.PerformOperation(operation);
            Assert.AreEqual(expected, actual, 0.001, "Incorrect calculation result");
        }

        [TestMethod]
        [ExpectedException(typeof(AlreadyExistsOperationException))]
        public void AlreadyExistsOperationExceptionTest()
        {
            var calculator = new Calculator();

            calculator.DefineOperation("++", x => x + 1);
        }

        [TestMethod]
        [ExpectedException(typeof(NotFoundOperationException))]
        public void NotFoundOperationExceptionTest()
        {
            var calculator = new Calculator();
            var parser = new Parser();
            var evaluator = new Evaluator(calculator, parser);

            evaluator.Calculate(" && 23  12 ");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ArgumentOutOfRangeExceptionTest()
        {
            var calculator = new Calculator();
            var parser = new Parser();
            var evaluator = new Evaluator(calculator, parser);

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

            evaluator.Calculate("  sqrt  -4 ");
        }
    }
}
