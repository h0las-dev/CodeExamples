namespace Scene2d.Commands
{
    public class CopyFigureCommand : ICommand
    {
        private readonly string name;
        private readonly string copyName;

        public CopyFigureCommand(string name, string copyName)
        {
            this.copyName = copyName;
            this.name = name;
        }

        public void Apply(Scene scene)
        {
            scene.CopyFigure(this.name, this.copyName);
        }
    }
}
