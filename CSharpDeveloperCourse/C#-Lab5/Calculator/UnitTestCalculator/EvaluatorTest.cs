namespace UnitTestCalculator
{
    using System;
    using Calculator;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class EvaluatorTest
    {
        [TestMethod]
        public void CalculateTest()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");

            var calculator = new Calculator();
            var parser = new Parser();
            var evaluator = new Evaluator(calculator, parser);

            calculator.DefineOperation("mod", (x, y) => x % y);
            calculator.DefineOperation("--", x => x - 1);
            calculator.DefineOperation("^", (x, y) => Convert.ToDouble(Math.Pow(x, y)));

            var expected = "-2";
            var actual = evaluator.Calculate(" -- -1");
            Assert.AreEqual(expected, actual, "Incorrect calculation result");

            expected = "2";
            actual = evaluator.Calculate("  mod 6   4  ");
            Assert.AreEqual(expected, actual, "Incorrect calculation result");

            expected = "0.125";
            actual = evaluator.Calculate(" ^   2   -3  ");
            Assert.AreEqual(expected, actual, "Incorrect calculation result");

            expected = "-7";
            actual = evaluator.Calculate(" T5   2 -7  4 ");
            Assert.AreEqual(expected, actual, "Incorrect calculation result");
        }
    }
}
