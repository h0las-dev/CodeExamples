namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class DeleteCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"delete\s\w+\s?(($)|(#.+$))");
        private string name;

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
                var regexName = new Regex(@"delete\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"(delete|DELETE)\s", string.Empty);
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
                return new DeleteSceneCommand();
            }

            return new DeleteFigureCommand(this.name);
        }
    }
}