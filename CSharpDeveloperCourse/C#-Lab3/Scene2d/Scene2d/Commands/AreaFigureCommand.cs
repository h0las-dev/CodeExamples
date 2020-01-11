namespace Scene2d.Commands
{
    public class AreaFigureCommand : ICommand
    {
        private readonly string name;

        public AreaFigureCommand(string name)
        {
            this.name = name;
        }

        public void Apply(Scene scene)
        {
            scene.PrintFigureArea(this.name);
        }
    }
}