namespace Calculator.Exceptions
{
    using System;

    public class IncorrectParametersException : Exception
    {
        public override string Message
        {
            get { return "Error: incorrect parameters!"; }
        }
    }
}
