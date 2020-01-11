namespace Scene2d.CommandBuilders
{
    using System;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;
    using Figures;

    public class AddCircleCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"add circle \w+ \(\s?-?\d+\s?,\s?-?\d+\s?\) radius \d+\s?(($)|(#.+$))");
        private CircleFigure circle;
        private string name;

        public bool IsCommandReady
        {
            get
            {
                if (this.circle != null && this.name != string.Empty)
                {
                    return true;
                }

                return false;
            }
        }

        public void AppendLine(string line)
        {
            line = Regex.Replace(line, @"\s+", " ").Trim();

            if (recognizeRegex.IsMatch(line.ToLower()))
            {
                var regexName = new Regex(@"\s\w+\s\(");
                var matchName = regexName.Match(line);

                this.name = Regex.Replace(matchName.Value, @"\(", string.Empty).Trim();

                var regexPoint = new Regex(@"\(-?\d+,-?\d+\)");
                var matchPoint = regexPoint.Match(Regex.Replace(line.ToLower(), @"\s+", string.Empty));
                var pointStr = Regex.Replace(matchPoint.Value, @"\(|\)", string.Empty);
                var coordinates = pointStr.Split(',');
                var point = new Point
                {
                    X = Convert.ToDouble(coordinates[0]),
                    Y = Convert.ToDouble(coordinates[1])
                };

                var regexRadius = new Regex(@"radius \d+\s?");
                var matchRadius = regexRadius.Match(line.ToLower());
                var radiusStr = Regex.Replace(matchRadius.Value.ToLower(), @"(radius|\s+)", string.Empty);
                var radius = Convert.ToDouble(radiusStr);

                this.circle = new CircleFigure(point, radius);
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        public ICommand GetCommand()
        {
            return new AddFigureCommand(this.name, this.circle);
        }
    }
}