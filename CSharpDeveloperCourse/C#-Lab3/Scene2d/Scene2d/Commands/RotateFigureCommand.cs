namespace Scene2d.Commands
{
    public class RotateFigureCommand : ICommand
    {
        private readonly string name;
        private readonly double angle;

        public RotateFigureCommand(string name, double angle)
        {
            this.name = name;
            this.angle = angle;
        }

        public void Apply(Scene scene)
        {
            scene.RotateFigure(this.name, this.angle);
        }
    }
}