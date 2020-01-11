namespace Scene2d.Commands
{
    public class ReflectSceneCommand : ICommand
    {
        private readonly string direction;

        public ReflectSceneCommand(string direction)
        {
            this.direction = direction;
        }

        public void Apply(Scene scene)
        {
            scene.ReflectScene(this.direction);
        }
    }
}