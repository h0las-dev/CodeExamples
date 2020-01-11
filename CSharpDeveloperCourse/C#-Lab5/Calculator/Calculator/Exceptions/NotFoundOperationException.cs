namespace Calculator.Exceptions
{
    using System;

    public class NotFoundOperationException : Exception 
    {
        public override string Message
        {
            get { return "Error: this operation is not found!"; }
        }
    }
}