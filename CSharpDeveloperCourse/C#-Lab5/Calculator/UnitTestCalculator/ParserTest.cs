namespace UnitTestCalculator
{
    using Calculator;
    using Calculator.Exceptions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void UnaryParserTest()
        {
            var expected = new Operation { Sign = "++", Parameters = new[] { 3.0 } };

            var parser = new Parser();
            var actual = parser.Parse(" ++  3");

            Assert.AreEqual(expected.Sign, actual.Sign, "Sign incorrectly defined");
            Assert.AreEqual(expected.Parameters[0], actual.Parameters[0], 0.001, "First option incorrectly defined");
        }

        [TestMethod]
        public void BinaryParserTest()
        {
            var expected = new Operation { Sign = "-", Parameters = new[] { 5.0, 3.0 } };

            var parser = new Parser();
            var actual = parser.Parse("- 5   3 ");

            Assert.AreEqual(expected.Sign, actual.Sign, "Sign incorrectly defined");
            Assert.AreEqual(expected.Parameters[0], actual.Parameters[0], 0.001, "First option incorrectly defined");
            Assert.AreEqual(expected.Parameters[1], actual.Parameters[1], 0.001, "Second option incorrectly defined");
        }

        [TestMethod]
        public void TernaryParserTest()
        {
            var expected = new Operation { Sign = "T5", Parameters = new[] { 5.0, 3.0, 4.0 } };

            var parser = new Parser();
            var actual = parser.Parse(" T5 5 3   4 ");

            Assert.AreEqual(expected.Sign, actual.Sign, "Sign incorrectly defined");
            Assert.AreEqual(expected.Parameters[0], actual.Parameters[0], 0.001, "First option incorrectly defined");
            Assert.AreEqual(expected.Parameters[1], actual.Parameters[1], 0.001, "Second option incorrectly defined");
            Assert.AreEqual(expected.Parameters[2], actual.Parameters[2], "Third option incorrectly defined");
        }

        [TestMethod]
        [ExpectedException(typeof(UnknownOperationTypeException))]
        public void UnknownOperationTypeExceptionTest()
        {
            var calculator = new Calculator();
            var parser = new Parser();
            var evaluator = new Evaluator(calculator, parser);

            evaluator.Calculate("2+3-1");
        }

        [TestMethod]
        [ExpectedException(typeof(IncorrectParametersException))]
        public void IncorrectParametersExceptionTest()
        {
            var parser = new Parser();

            parser.Parse("  5 + A");
        }
    }
}