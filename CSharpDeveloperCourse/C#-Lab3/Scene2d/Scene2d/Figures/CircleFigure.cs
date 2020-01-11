namespace Scene2d.Figures
{
    using System;
    using Exceptions;

    public class CircleFigure : IFigure
    {
        private Circle circleStruct;

        public CircleFigure(Point p1, double radius)
        {
            this.circleStruct.Vertex = p1;
            if (radius <= 0)
            {
                throw new BadCircleRadiusExeption("bad circle radius");
            }
            else
            {
                this.circleStruct.Radius = radius;
            }
        }
        
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public double CalulateArea()
        {
            return Math.PI * this.circleStruct.Radius * this.circleStruct.Radius;
        }

        public Rectangle CalculateCircumscribingRectangle()
        {
            var circRectangleVertex1 = new Point();
            var circRectangleVertex2 = new Point();

            circRectangleVertex1.X = this.circleStruct.Vertex.X - this.circleStruct.Radius;
            circRectangleVertex1.Y = this.circleStruct.Vertex.Y - this.circleStruct.Radius;

            circRectangleVertex2.X = this.circleStruct.Vertex.X + this.circleStruct.Radius;
            circRectangleVertex2.Y = this.circleStruct.Vertex.Y + this.circleStruct.Radius;

            return new Rectangle(circRectangleVertex1, circRectangleVertex2);
        }

        public void Move(Point vector)
        {
            var movePoint = new Point
            {
                X = this.circleStruct.Vertex.X + vector.X,
                Y = this.circleStruct.Vertex.Y + vector.Y
            };

            this.circleStruct.Vertex = movePoint;
        }

        public void Rotate(double angle)
        {
            // Nothing to do.
        }

        public void Reflect(string direction)
        {
            // Nothing to do.
        }
    }
}