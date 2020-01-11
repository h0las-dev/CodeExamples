namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class ReflectFigureCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"reflect\s(vertically|horizontally)\s\w+\s?(($)|(#.+$))");
        private string name;
        private string direction;

        public bool IsCommandReady
        {
            get
            {
                if (this.name != string.Empty && this.direction != string.Empty)
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
                var regexName = new Regex(@"\s(vertically|horizontally|VERTICALLY|HORIZONTALLY)\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"\s(vertically|horizontally|VERTICALLY|HORIZONTALLY)\s", string.Empty);

                var regexDirection = new Regex(@"\s(vertically|horizontally)\s");
                var matchDirection = regexDirection.Match(line.ToLower());
                this.direction = Regex.Replace(matchDirection.Value, @"\s", string.Empty);
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
                return new ReflectSceneCommand(this.direction);
            }
            else
            {
                return new ReflectFigureCommand(this.name, this.direction);
            }
        }
    }
}