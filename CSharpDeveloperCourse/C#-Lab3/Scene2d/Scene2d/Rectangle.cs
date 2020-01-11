namespace Scene2d
{
    using System;
    using Exceptions;

    public struct Rectangle
    {
        public Rectangle(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) < 1e-10 || Math.Abs(p1.Y - p2.Y) < 1e-10)
            {
                throw new BadRectanglePointExeption("bad rectangle point");
            }

            this.Vertex1 = p1;
            this.Vertex2 = p2;

            var p3 = new Point
            {
                X = p2.X,
                Y = p1.Y
            };

            var p4 = new Point
            {
                X = p1.X,
                Y = p2.Y
            };

            this.Vertex3 = p3;
            this.Vertex4 = p4;
        }

        public Point Vertex1 { get; set; }

        public Point Vertex2 { get; set; }

        public Point Vertex3 { get; set; }

        public Point Vertex4 { get; set; }
    }
}