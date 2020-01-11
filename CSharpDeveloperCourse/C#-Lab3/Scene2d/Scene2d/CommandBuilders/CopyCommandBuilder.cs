namespace Scene2d.CommandBuilders
{
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class CopyCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"copy\s\w+\sto\s\w+\s?(($)|(#.+$))");
        private string copyName;
        private string name;
            
        public bool IsCommandReady
        {
            get
            {
                if (this.copyName != string.Empty && this.name != string.Empty)
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
                var regexName = new Regex(@"(copy|COPY)\s\w+");
                var matchName = regexName.Match(line);
                this.name = Regex.Replace(matchName.Value, @"(copy|COPY)\s", string.Empty);

                var regexCopyName = new Regex(@"\s(to|TO)\s\w+\s?");
                var matchCopyName = regexCopyName.Match(line);
                this.copyName = Regex.Replace(matchCopyName.Value, @"\s(to|TO)\s", string.Empty);
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
                return new CopySceneCommand(this.copyName);
            }

            return new CopyFigureCommand(this.name, this.copyName);
        }
    }
}