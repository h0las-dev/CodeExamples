namespace Scene2d
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using System.Text.RegularExpressions;

    public class CommandProducer
    {
        static Dictionary<Regex, ICommandBuilder> commands = new Dictionary<Regex, ICommandBuilder>
        {
            { new Regex("add rectangle.*"), new AddRectangleCommandBuilder() }
        };

        ICommandBuilder currentBuilder;

        public void AppendLine(string line)
        {
            if (currentBuilder == null)
            {
                foreach (var pair in commands.eleme
            }

        }

        public bool IsCommandReady { get; set; }

        public object GetCommand()
        {
            throw new NotImplementedException();
        }
    }
}
