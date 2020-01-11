namespace Scene2d.Exceptions
{
    using System;

    public class UnexpectedEndOfPolygonExeption : Exception
    {
        public UnexpectedEndOfPolygonExeption(string message) : base(message)
        {
        }
    }
}