namespace Scene2d.CommandBuilders
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class AddPolygonCommandBuilder : ICommandBuilder
    {
        private static readonly Regex AddPolygonRegex = new Regex(@"(add polygon \s?\w+\s?(($)|(#.+$)))");
        private static readonly Regex EndPolygonRegex = new Regex(@"(end polygon\s?)(($)|(#.+$))");
        private static readonly Regex AddPointRegex = new Regex(@"add point \(\s?-?\d+\s?,\s?-?\d+\s?\)(($)|(#.+$))");
        private bool isPolygonComplete = false;
        private List<Point> points = new List<Point>();
        private string name = string.Empty;

        public bool IsCommandReady
        {
            get
            {
                if (this.name != string.Empty && this.isPolygonComplete && this.points.Count >= 3)
                {
                    return true;
                }

                return false;
            }
        }

        public void AppendLine(string line)
        {
            line = Regex.Replace(line, @"\s+", " ").Trim();

            if (AddPolygonRegex.IsMatch(line.ToLower()))
            {
                if (this.name != string.Empty)
                {
                    throw new BadFormatException("can't create polygon more than once");
                }

                this.name = Regex.Replace(line, @"(add\spolygon\s)|(ADD\sPOLYGON\s)", string.Empty);
            }
            else if (EndPolygonRegex.IsMatch(line.ToLower()))
            {
                if (!this.isPolygonComplete)
                {
                    this.isPolygonComplete = true;
                }
                else
                {
                    throw new BadFormatException("can't create polygon more than once");
                }
            }
            else if (AddPointRegex.IsMatch(line.ToLower()))
            {
                var regexPoint = new Regex(@"\(-?\d+,-?\d+\)");
                var matchPoint = regexPoint.Match(Regex.Replace(line.ToLower(), @"\s+", string.Empty));
                var pointStr = Regex.Replace(matchPoint.Value, @"\(|\)", string.Empty);
                var coordinates = pointStr.Split(',');
                var point = new Point
                {
                    X = Convert.ToDouble(coordinates[0]),
                    Y = Convert.ToDouble(coordinates[1])
                };

                this.points.Add(point);
            }
            else
            {
                throw new BadFormatException("unexpected end of polygon");
            }
        }

        public ICommand GetCommand()
        {
            return new AddPolygonCommand(this.name, this.points);
        }
    }
}