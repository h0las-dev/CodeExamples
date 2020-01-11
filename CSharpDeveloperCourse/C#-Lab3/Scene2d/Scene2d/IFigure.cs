namespace Scene2d
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    internal interface IFigure
    {
        double CalulateArea();

        Rectangle CalculateCircumscribingRectangle();
    }
}
