namespace Scene2d.Figures
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class RectangleFigure : IFigure
    {
        public const double Eps = 1e-10;

        private Rectangle rectangleStruct;

        public RectangleFigure(Point p1, Point p2)
        {
            if (Math.Abs(p1.X - p2.X) > Eps && Math.Abs(p1.Y - p2.Y) > Eps)
            {
                this.rectangleStruct = new Rectangle(p1, p2);
            }
            else
            {
                throw new BadRectanglePointExeption("bad rectangle point exeption");
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public double CalulateArea()
        {
            var length = Math.Sqrt(Math.Pow(this.rectangleStruct.Vertex1.X - this.rectangleStruct.Vertex3.X, 2) + Math.Pow(this.rectangleStruct.Vertex1.Y - this.rectangleStruct.Vertex3.Y, 2));
            var width = Math.Sqrt(Math.Pow(this.rectangleStruct.Vertex1.Y - this.rectangleStruct.Vertex4.Y, 2) + Math.Pow(this.rectangleStruct.Vertex1.X - this.rectangleStruct.Vertex4.X, 2));

            var area = length * width;

            return area;
        }

        public Rectangle CalculateCircumscribingRectangle()
        {
            var points = new List<Point>();
            points.Add(this.rectangleStruct.Vertex1);
            points.Add(this.rectangleStruct.Vertex2);
            points.Add(this.rectangleStruct.Vertex3);
            points.Add(this.rectangleStruct.Vertex4);

            var minX = 0.0;
            var minY = 0.0;
            var maxX = 0.0;
            var maxY = 0.0;

            for (var i = 0; i < points.Count; i++)
            {
                if (i == 0)
                {
                    minX = points[i].X;
                    maxX = points[i].X;
                    minY = points[i].Y;
                    maxY = points[i].Y;
                }

                if (points[i].X < minX)
                {
                    minX = points[i].X;
                }

                if (points[i].X > maxX)
                {
                    maxX = points[i].X;
                }

                if (points[i].Y < minY)
                {
                    minY = points[i].Y;
                }

                if (points[i].Y > maxY)
                {
                    maxY = points[i].Y;
                }
            }

            var vertex1 = new Point
            {
                X = minX,
                Y = minY
            };
            var vertex2 = new Point
            {
                X = maxX,
                Y = maxY
            };

            return new Rectangle(vertex1, vertex2);
        }

        public void Move(Point vector)
        {
            var vertex1 = new Point
            {
                X = this.rectangleStruct.Vertex1.X + vector.X,
                Y = this.rectangleStruct.Vertex1.Y + vector.Y
            };

            var vertex2 = new Point
            {
                X = this.rectangleStruct.Vertex2.X + vector.X,
                Y = this.rectangleStruct.Vertex2.Y + vector.Y
            };

            var vertex3 = new Point
            {
                X = this.rectangleStruct.Vertex3.X + vector.X,
                Y = this.rectangleStruct.Vertex3.Y + vector.Y
            };

            var vertex4 = new Point
            {
                X = this.rectangleStruct.Vertex4.X + vector.X,
                Y = this.rectangleStruct.Vertex4.Y + vector.Y
            };

            this.rectangleStruct.Vertex1 = vertex1;
            this.rectangleStruct.Vertex2 = vertex2;
            this.rectangleStruct.Vertex3 = vertex3;
            this.rectangleStruct.Vertex4 = vertex4;
        }

        // The rotate is clockwise!
        public void Rotate(double angle)
        {
            var radAngle = angle * Math.PI / 180;

            var xCenter = (this.rectangleStruct.Vertex2.X + this.rectangleStruct.Vertex1.X) / 2;
            var yCenter = (this.rectangleStruct.Vertex2.Y + this.rectangleStruct.Vertex1.Y) / 2;

            var points = new List<Point>();
            points.Add(this.rectangleStruct.Vertex1);
            points.Add(this.rectangleStruct.Vertex2);
            points.Add(this.rectangleStruct.Vertex3);
            points.Add(this.rectangleStruct.Vertex4);

            var centerPoints = new List<Point>();

            for (var i = 0; i < points.Count; i++)
            {
                var centerVertex = new Point
                {
                    X = points[i].X - xCenter,
                    Y = points[i].Y - yCenter
                };

                centerPoints.Add(centerVertex);
            }

            for (var i = 0; i < points.Count; i++)
            {
                var newCenterVertex = new Point()
                {
                    X = Math.Round((centerPoints[i].X * Math.Cos(radAngle)) + (centerPoints[i].Y * Math.Sin(radAngle)) + xCenter, 5),
                    Y = Math.Round((-centerPoints[i].X * Math.Sin(radAngle)) + (centerPoints[i].Y * Math.Cos(radAngle)) + yCenter, 5)
                };

                points[i] = newCenterVertex;
            }

            this.rectangleStruct.Vertex1 = points[0];
            this.rectangleStruct.Vertex2 = points[1];
            this.rectangleStruct.Vertex3 = points[2];
            this.rectangleStruct.Vertex4 = points[3];
        }

        public void Reflect(string direction)
        {
            var points = new List<Point>();

            points.Add(this.rectangleStruct.Vertex1);
            points.Add(this.rectangleStruct.Vertex4);
            points.Add(this.rectangleStruct.Vertex2);
            points.Add(this.rectangleStruct.Vertex3);

            var centerPoint = new Point()
            {
                X = (this.rectangleStruct.Vertex2.X + this.rectangleStruct.Vertex1.X) / 2,
                Y = (this.rectangleStruct.Vertex2.Y + this.rectangleStruct.Vertex1.Y) / 2
            };

            if (direction == "horizontally")
            {
                for (var i = 0; i < points.Count; i++)
                {
                    if (points[i].Y > centerPoint.Y)
                    {
                        var reflectPoint = new Point()
                        {
                            X = points[i].X,
                            Y = centerPoint.Y - (points[i].Y - centerPoint.Y)
                        };

                        points[i] = reflectPoint;
                    }
                    else if (points[i].Y < centerPoint.Y)
                    {
                        var reflectPoint = new Point()
                        {
                            X = points[i].X,
                            Y = centerPoint.Y + (centerPoint.Y - points[i].Y)
                        };

                        points[i] = reflectPoint;
                    }
                    else
                    {
                        points[i] = points[i];
                    }
                }
            }
            else if (direction == "vertically")
            {
                for (var i = 0; i < points.Count; i++)
                {
                    if (points[i].X > centerPoint.X)
                    {
                        var reflectPoint = new Point()
                        {
                            X = centerPoint.X - (points[i].X - centerPoint.X),
                            Y = points[i].Y
                        };

                        points[i] = reflectPoint;
                    }
                    else if (points[i].X < centerPoint.X)
                    {
                        var reflectPoint = new Point()
                        {
                            X = centerPoint.X + (centerPoint.X - points[i].X),
                            Y = points[i].Y
                        };

                        points[i] = reflectPoint;
                    }
                    else
                    {
                        points[i] = points[i];
                    }
                }
            }
            else
            {
                throw new BadFormatException("bad format");
            }

            this.rectangleStruct.Vertex1 = points[0];
            this.rectangleStruct.Vertex2 = points[2];
            this.rectangleStruct.Vertex3 = points[3];
            this.rectangleStruct.Vertex4 = points[1];
        }
    }
}