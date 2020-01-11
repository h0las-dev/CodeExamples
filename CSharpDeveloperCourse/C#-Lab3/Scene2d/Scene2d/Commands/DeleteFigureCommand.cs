namespace Scene2d.Commands
{
    public class DeleteFigureCommand : ICommand
    {
        private readonly string name;

        public DeleteFigureCommand(string name)
        {
            this.name = name;
        }

        public void Apply(Scene scene)
        {
            scene.DeleteFigure(this.name);
        }
    }
}