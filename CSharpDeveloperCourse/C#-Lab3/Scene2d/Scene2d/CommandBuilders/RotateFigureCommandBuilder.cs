namespace Scene2d.CommandBuilders
{
    using System;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class RotateFigureCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"rotate\s\w+\s\d+\s?(($)|(#.+$))");
        private string name;
        private double angle;

        public bool IsCommandReady
        {
            get
            {
                if (this.name != string.Empty && Math.Abs(this.angle) > 1e-9)
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
                var regexName = new Regex(@"(rotate|ROTATE)\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"(rotate|ROTATE)\s", string.Empty);

                var regexAngle = new Regex(@"\s\d+");
                var matchAngle = regexAngle.Match(line);
                this.angle = Convert.ToDouble(matchAngle.Value.Trim());
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
                return new RotateSceneCommand(this.angle);
            }
            else
            {
                return new RotateFigureCommand(this.name, this.angle);
            }
        }
    }
}