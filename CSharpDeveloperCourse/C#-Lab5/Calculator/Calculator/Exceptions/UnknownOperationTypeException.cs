namespace Calculator.Exceptions
{
    using System;

    public class UnknownOperationTypeException : Exception
    {
        public override string Message
        {
            get { return "Error: unknown operation type!"; }
        }
    }
}
