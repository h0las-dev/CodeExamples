namespace Scene2d.Commands
{
    using System.Collections.Generic;

    public class AddGroupCommand : ICommand
    {
        private readonly string name;
        private readonly List<string> figureNames;

        public AddGroupCommand(string name, List<string> names)
        {
            this.name = name;
            this.figureNames = new List<string>(names);
        }

        public void Apply(Scene scene)
        {
            scene.CreateCompositeFigure(this.name, this.figureNames);
        }
    }
}
