namespace Scene2d.Figures
{
    using System;
    using System.Collections.Generic;
    using Exceptions;

    public class PolygonFigure : IFigure
    {
        public const double Eps = 1e-10;

        private readonly List<Point> points;

        public PolygonFigure(string name, List<Point> points)
        {
            this.Name = name;
            this.points = new List<Point>(points);
        }

        public string Name { get; set; }

        public void CheckPolygon()
        {
            if (this.points.Count == 3)
            {
                if (Math.Abs(((this.points[2].X - this.points[0].X) / (this.points[1].X - this.points[0].X)) - ((this.points[2].Y - this.points[0].Y) / (this.points[1].Y - this.points[0].Y))) < Eps)
                {
                    throw new BadFormatException("bad polygon point");
                }
            }

            for (var i = 0; i < this.points.Count; i++)
            {
                var currentPoint1 = new Point();
                var currentPoint2 = new Point();

                if (i != this.points.Count - 1)
                {
                    currentPoint1 = this.points[i];
                    currentPoint2 = this.points[i + 1];
                }
                else
                {
                    currentPoint1 = this.points[i];
                    currentPoint2 = this.points[0];
                }

                for (var j = 0; j < this.points.Count; j++)
                {
                    if (j >= i + 2 || (j <= i - 2 && i - 2 >= 0))
                    {
                        if (j == 0 && i == this.points.Count - 1)
                        {
                            continue;
                        }
                        
                        var tmpPoint1 = new Point();
                        var tmpPoint2 = new Point();

                        if (j != this.points.Count - 1)
                        {
                            tmpPoint1 = this.points[j];
                            tmpPoint2 = this.points[j + 1];
                        }
                        else
                        {
                            if (i != 0)
                            {
                                tmpPoint1 = this.points[j];
                                tmpPoint2 = this.points[0];
                            }
                            else
                            {
                                continue;
                            }
                        }

                        if (this.CheckIntersaction(currentPoint1, currentPoint2, tmpPoint1, tmpPoint2))
                        {
                            throw new BadFormatException("bad polygon point");
                        }
                    }
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public double CalulateArea()
        {
            var s = 0.0;

            for (var i = 0; i < this.points.Count; i++)
            {
                if (i != this.points.Count - 1)
                {
                    s += (this.points[i].X + this.points[i + 1].X) * (this.points[i].Y - this.points[i + 1].Y);
                }
                else
                {
                    s += (this.points[i].X + this.points[0].X) * (this.points[i].Y - this.points[0].Y);
                }
            }

            s *= 0.5;

            return Math.Abs(s);
        }

        public Rectangle CalculateCircumscribingRectangle()
        {
            var minX = 0.0;
            var minY = 0.0;
            var maxX = 0.0;
            var maxY = 0.0;

            for (var i = 0; i < this.points.Count; i++)
            {
                if (i == 0)
                {
                    minX = this.points[i].X;
                    maxX = this.points[i].X;
                    minY = this.points[i].Y;
                    maxY = this.points[i].Y;
                }

                if (this.points[i].X < minX)
                {
                    minX = this.points[i].X;
                }

                if (this.points[i].X > maxX)
                {
                    maxX = this.points[i].X;
                }

                if (this.points[i].Y < minY)
                {
                    minY = this.points[i].Y;
                }

                if (this.points[i].Y > maxY)
                {
                    maxY = this.points[i].Y;
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
            for (var i = 0; i < this.points.Count; i++)
            {
                var vertex = new Point
                {
                    X = this.points[i].X + vector.X,
                    Y = this.points[i].Y + vector.Y
                };

                this.points[i] = vertex;
            }
        }

        // The rotate is clockwise!
        public void Rotate(double angle)
        {
            var radAngle = angle * Math.PI / 180;

            var xCenter = 0.0;
            var yCenter = 0.0;

            for (var i = 0; i < this.points.Count; i++)
            {
                xCenter += this.points[i].X;
                yCenter += this.points[i].Y;
            }

            xCenter /= this.points.Count;
            yCenter /= this.points.Count;

            var centerPoints = new List<Point>();

            for (var i = 0; i < this.points.Count; i++)
            {
                var centerVertex = new Point
                {
                    X = this.points[i].X - xCenter,
                    Y = this.points[i].Y - yCenter
                };

                centerPoints.Add(centerVertex);
            }

            for (var i = 0; i < this.points.Count; i++)
            {
                var newCenterVertex = new Point()
                {
                    X = Math.Round((centerPoints[i].X * Math.Cos(radAngle)) + (centerPoints[i].Y * Math.Sin(radAngle)) + xCenter, 5),
                    Y = Math.Round((-centerPoints[i].X * Math.Sin(radAngle)) + (centerPoints[i].Y * Math.Cos(radAngle)) + yCenter, 5)
                };

                this.points[i] = newCenterVertex;
            }
        }

        public void Reflect(string direction)
        {
            var xCenter = 0.0;
            var yCenter = 0.0;

            for (var i = 0; i < this.points.Count; i++)
            {
                xCenter += this.points[i].X;
                yCenter += this.points[i].Y;
            }

            xCenter /= this.points.Count;
            yCenter /= this.points.Count;

            var centerPoint = new Point()
            {
                X = xCenter,
                Y = yCenter
            };

            if (direction == "horizontally")
            {
                for (var i = 0; i < this.points.Count; i++)
                {
                    if (this.points[i].Y > centerPoint.Y)
                    {
                        var reflectPoint = new Point()
                        {
                            X = this.points[i].X,
                            Y = centerPoint.Y - (this.points[i].Y - centerPoint.Y)
                        };

                        this.points[i] = reflectPoint;
                    }
                    else if (this.points[i].Y < centerPoint.Y)
                    {
                        var reflectPoint = new Point()
                        {
                            X = this.points[i].X,
                            Y = centerPoint.Y + (centerPoint.Y - this.points[i].Y)
                        };

                        this.points[i] = reflectPoint;
                    }
                    else 
                    {
                        this.points[i] = this.points[i];
                    }
                }
            }
            else if (direction == "vertically")
            {
                for (var i = 0; i < this.points.Count; i++)
                {
                    if (this.points[i].X > centerPoint.X)
                    {
                        var reflectPoint = new Point()
                        {
                            X = centerPoint.X - (this.points[i].X - centerPoint.X),
                            Y = this.points[i].Y
                        };

                        this.points[i] = reflectPoint;
                    }
                    else if (this.points[i].X < centerPoint.X)
                    {
                        var reflectPoint = new Point()
                        {
                            X = centerPoint.X + (centerPoint.X - this.points[i].X),
                            Y = this.points[i].Y
                        };

                        this.points[i] = reflectPoint;
                    }
                    else
                    {
                        this.points[i] = this.points[i];
                    }
                }
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        private static void Swap(ref double a, ref double b)
        {
            var c = a;
            a = b;
            b = c;
        }

        private static double GetTriangleArea(Point a, Point b, Point c)
        {
            return ((b.X - a.X) * (c.Y - a.Y)) - ((b.Y - a.Y) * (c.X - a.X));
        }

        private bool CheckIntersaction(Point a, Point b, Point c, Point d)
        {
            return this.IntersectBox(a.X, b.X, c.X, d.X) && this.IntersectBox(a.Y, b.Y, c.Y, d.Y) &&
                   GetTriangleArea(a, b, c) * GetTriangleArea(a, b, d) < Eps &&
                   GetTriangleArea(c, d, a) * GetTriangleArea(c, d, b) < Eps;
        }

        private bool IntersectBox(double a, double b, double c, double d)
        {
            if (a > b)
            {
                Swap(ref a, ref b);
            }

            if (c > d)
            {
                Swap(ref c, ref d);
            }

            return Math.Max(a, c) <= Math.Min(b, d);
        }
    }
}