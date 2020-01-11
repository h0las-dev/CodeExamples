namespace Scene2d
{
    using System;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;
    using Figures;

    public class AddRectangleCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"add rectangle \s?\w+\s? \(\s?-?\d+\s?,\s?-?\d+\s?\) \(\s?-?\d+\s?,\s?-?\d+\s?\)\s?(($)|(#.+$))");
        private RectangleFigure rectangle;
        private string name;

        public bool IsCommandReady
        {
            get
            {
                if (this.rectangle != null && this.name != string.Empty)
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
                var pointStr1 = Regex.Replace(matchPoint.Value, @"\(|\)", string.Empty);
                var coordinates1 = pointStr1.Split(',');
                var point1 = new Point
                {
                    X = Convert.ToDouble(coordinates1[0]),
                    Y = Convert.ToDouble(coordinates1[1])
                };

                matchPoint = matchPoint.NextMatch();
                var pointStr2 = Regex.Replace(matchPoint.Value, @"\(|\)", string.Empty);
                var coordinates2 = pointStr2.Split(',');
                var point2 = new Point
                {
                    X = Convert.ToDouble(coordinates2[0]),
                    Y = Convert.ToDouble(coordinates2[1])
                };

                this.rectangle = new RectangleFigure(point1, point2);
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        public ICommand GetCommand()
        {
            return new AddFigureCommand(this.name, this.rectangle);
        }
    }
}