namespace Scene2d.Commands
{
    public class CircRectangleCommand : ICommand
    {
        private readonly string name;

        public CircRectangleCommand(string name)
        {
            this.name = name;
        }

        public void Apply(Scene scene)
        {
            scene.PrintCircumscribingRectangle(this.name);
        }
    }
}