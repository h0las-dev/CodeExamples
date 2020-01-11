namespace Scene2d
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using CommandBuilders;
    using Exceptions;

    public class CommandProducer : ICommandBuilder
    {
        // ----- members -----------------------------------------------------------------
        private static Dictionary<Regex, Func<ICommandBuilder>> commands =
            new Dictionary<Regex, Func<ICommandBuilder>>
            {
                { new Regex(@"\s*(add|ADD)\s*(rectangle|RECTANGLE)"), () => new AddRectangleCommandBuilder() },
                { new Regex(@"\s*(print|PRINT)\s*(area|AREA)\s*(for|FOR)\s*"), () => new AreaCommandBuilder() },
                { new Regex(@"\s*(delete|DELETE)\s*"), () => new DeleteCommandBuilder() },
                { new Regex(@"\s*(print|PRINT)\s*(circumscribing|CIRCUMSCRIBING)\s*(rectangle|RECTANGLE)\s*(for|FOR)\s*"), () => new CircRectangleCommandBuilder() },
                { new Regex(@"\s*(move|MOVE)\s*"), () => new MoveCommandBuilder() },
                { new Regex(@"\s*(group.+as|GROUP.+AS)\s*"), () => new GroupCommandBuilder() },
                { new Regex(@"\s*(copy|COPY)\s*"), () => new CopyCommandBuilder() },
                { new Regex(@"\s*(add|ADD)\s*(circle|CIRCLE)"), () => new AddCircleCommandBuilder() },
                { new Regex(@"\s*(add|ADD)\s*(polygon|POLYGON)"), () => new AddPolygonCommandBuilder() },
                { new Regex(@"\s*(rotate|ROTATE)\s*"), () => new RotateFigureCommandBuilder() },
                { new Regex(@"\s*(reflect|REFLECT)\s*vertically|VERTICALLY|horizontally|HORIZONTALLY\s*"), () => new ReflectFigureCommandBuilder() }
            };

        private ICommandBuilder currentBuilder;

        // ----- public ------------------------------------------------------------------
        public bool IsCommandReady
        {
            get
            {
                if (this.currentBuilder == null)
                {
                    return false;
                }

                return this.currentBuilder.IsCommandReady;
            }
        }

        public void AppendLine(string line)
        {
            if (this.currentBuilder == null)
            {
                foreach (var pair in commands)
                {
                    if (pair.Key.IsMatch(line))
                    {
                        this.currentBuilder = pair.Value();
                        break;
                    }
                }
            }

            if (this.currentBuilder == null)
            {
                throw new BadFormatException("bad format");
            }

            this.currentBuilder.AppendLine(line);
        }

        public ICommand GetCommand()
        {
            if (this.currentBuilder == null)
            {
                return null;
            }

            var command = this.currentBuilder.GetCommand();
            this.currentBuilder = null;

            return command;
        }
    }
}