namespace Scene2d.Commands
{
    public class MoveSceneCommand : ICommand
    {
        private readonly Point vector;

        public MoveSceneCommand(Point vector)
        {
            this.vector = vector;
        }

        public void Apply(Scene scene)
        {
            scene.MoveScene(this.vector);
        }
    }
}