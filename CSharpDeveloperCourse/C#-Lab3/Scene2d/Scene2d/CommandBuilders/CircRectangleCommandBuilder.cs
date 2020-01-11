namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class CircRectangleCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"print circumscribing rectangle for\s\w+\s?(($)|(#.+$))");
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
                var regexName = new Regex(@"\s(for|FOR)\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"\s(for|FOR)\s", string.Empty);
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        public ICommand GetCommand()
        {
            if (this.name == "scene" || this.name == "SCENE")
            {
                return new CircSceneRectangleCommand();
            }
            else
            {
                return new CircRectangleCommand(this.name);
            }
        }
    }
}