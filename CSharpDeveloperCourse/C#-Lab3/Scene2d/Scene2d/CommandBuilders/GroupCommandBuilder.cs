namespace Scene2d.CommandBuilders
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Commands;
    using Exceptions;

    public class GroupCommandBuilder : ICommandBuilder
    {
        private static Regex recognizeRegex = new Regex(@"group (\w+,?\s?)+ as \w+(($)|(#.+$))");
        private string name;
        private List<string> figureNames;

        public bool IsCommandReady
        {
            get
            {
                if (this.name != string.Empty && this.figureNames.Count != 0)
                {
                    return true;
                }

                return false;
            }
        }

        public void AppendLine(string line)
        {
            this.figureNames = new List<string>();

            line = Regex.Replace(line, @"\s+", " ").Trim();
            if (recognizeRegex.IsMatch(line.ToLower()))
            {
                var regexName = new Regex(@"(as|AS) \w+");
                var matchName = regexName.Match(line);
                var groupName = matchName.Value;
                groupName = Regex.Replace(groupName, @"(as|AS|\s)", string.Empty);
                this.name = groupName;

                var regexFigureNames = new Regex(@"(group|GROUP)\s.+\s(as|AS)");
                var matchFigureNames = regexFigureNames.Match(line);
                var tmpNames = matchFigureNames.Value;
                tmpNames = Regex.Replace(tmpNames, @"((group|Group)\s)|(\s(as|AS))", string.Empty);
                tmpNames = Regex.Replace(tmpNames, @"\s+", string.Empty);

                var arrayFigureNames = tmpNames.Split(',');

                for (var i = 0; i < arrayFigureNames.Length; i++)
                {
                    this.figureNames.Add(arrayFigureNames[i]);
                }
            }
            else
            {
                throw new BadFormatException("bad format");
            }
        }

        public ICommand GetCommand()
        {
            return new AddGroupCommand(this.name, this.figureNames);
        }
    }
}