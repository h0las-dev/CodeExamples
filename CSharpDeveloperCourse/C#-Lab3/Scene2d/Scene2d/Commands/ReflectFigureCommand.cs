namespace Scene2d.Commands
{
    public class ReflectFigureCommand : ICommand
    {
        private readonly string name;
        private readonly string direction;

        public ReflectFigureCommand(string name, string direction)
        {
            this.name = name;
            this.direction = direction;
        }

        public void Apply(Scene scene)
        {
            scene.ReflectFigure(this.name, this.direction);
        }
    }
}