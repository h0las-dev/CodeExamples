namespace Scene2d.Figures
{
    using System;

    public interface IFigure : ICloneable
    {
        double CalulateArea();

        Rectangle CalculateCircumscribingRectangle();

        void Move(Point vector);

        void Rotate(double angle);

        void Reflect(string direction);
    }
}