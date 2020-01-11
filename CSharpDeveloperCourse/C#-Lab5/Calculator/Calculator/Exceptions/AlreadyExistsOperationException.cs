namespace Calculator.Exceptions
{
    using System;

    public class AlreadyExistsOperationException : Exception 
    {
        public override string Message
        {
            get { return "Error: this operation is already defined!"; }
        }
    }
}