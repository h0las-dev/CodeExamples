namespace Scene2d.Exceptions
{
    using System;

    public class NameDoesAlreadyExistExeption : Exception
    {
        public NameDoesAlreadyExistExeption(string message) : base(message)
        {
        }
    }
}