namespace Scene2d.CommandBuilders
{
    using System;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class MoveCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"move\s\w+\s\(\s?-?\d+\s?,\s?-?\d+\s?\)(($)|(#.+$))");
        private string name;
        private Point vector;

        public bool IsCommandReady
        {
            get
            {
                if (this.name != string.Empty)
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
                var regexName = new Regex(@"(move|MOVE)\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"(move|MOVE)\s", string.Empty);

                var regexPoint = new Regex(@"\(-?\d+,-?\d+\)");
                var matchPoint = regexPoint.Match(Regex.Replace(line, @"\s+", string.Empty));
                var point1 = Regex.Replace(matchPoint.Value, @"\(|\)", string.Empty);
                var coordinates1 = point1.Split(',');
                this.vector.X = Convert.ToDouble(coordinates1[0]);
                this.vector.Y = Convert.ToDouble(coordinates1[1]);
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        public ICommand GetCommand()
        {
            if (this.name.ToLower() == "scene")
            {
                return new MoveSceneCommand(this.vector);
            }
            else
            {
                return new MoveFigureCommand(this.name, this.vector);
            }
        }
    }
}