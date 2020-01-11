namespace Scene2d.Figures
{
    using System.Collections.Generic;

    internal class CompositeFigure : ICompositeFigure
    {
        private List<IFigure> figures = new List<IFigure>();

        public IList<IFigure> ChildFigures
        {
            get { return this.figures; }
            set { this.figures = new List<IFigure>(value); }
        }

        public double CalulateArea()
        {
            var s = 0.0;

            foreach (var figure in this.figures)
            {
                s += figure.CalulateArea();
            }

            return s;
        }

        public void AddFigure(IFigure figure)
        {
            this.figures.Add(figure);
        }

        public Rectangle CalculateCircumscribingRectangle()
        {
            var circRectangles = new List<Rectangle>();

            foreach (var figure in this.figures)
            {
                circRectangles.Add(figure.CalculateCircumscribingRectangle());
            }

            return this.FindCircRectangle(circRectangles);
        }

        public void Move(Point vector)
        {
            foreach (var figure in this.figures)
            {
                figure.Move(vector);
            }
        }

        public void Rotate(double angle)
        {
            foreach (var figure in this.ChildFigures)
            {
                figure.Rotate(angle);
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public void Reflect(string direction)
        {
            foreach (var figure in this.ChildFigures)
            {
                figure.Reflect(direction);
            }
        }

        private Rectangle FindCircRectangle(List<Rectangle> rectangles)
        {
            var xMin = 0.0;
            var yMin = 0.0;
            var xMax = 0.0;
            var yMax = 0.0;
            var counter = 0;
            
            foreach (var rectangle in rectangles)
            {
                var currentLeftPoint = rectangle.Vertex1;
                var currentRightPoint = rectangle.Vertex2;

                if (counter == 0)
                {
                    xMin = currentLeftPoint.X;
                    yMin = currentLeftPoint.Y;
                    xMax = currentRightPoint.X;
                    yMax = currentRightPoint.Y;
                }

                if (currentLeftPoint.X < xMin)
                {
                    xMin = currentLeftPoint.X;
                }

                if (currentLeftPoint.Y < yMin)
                {
                    yMin = currentLeftPoint.Y;
                }

                if (currentRightPoint.X > xMax)
                {
                    xMax = currentRightPoint.X;
                }

                if (currentRightPoint.Y > yMax)
                {
                    yMax = currentRightPoint.Y;
                }

                counter++;
            }

            var circRectangleVetrex1 = new Point();
            var circRectangleVertex2 = new Point();

            circRectangleVetrex1.X = xMin;
            circRectangleVetrex1.Y = yMin;
            circRectangleVertex2.X = xMax;
            circRectangleVertex2.Y = yMax;

            return new Rectangle(circRectangleVetrex1, circRectangleVertex2);
        }
    }
}