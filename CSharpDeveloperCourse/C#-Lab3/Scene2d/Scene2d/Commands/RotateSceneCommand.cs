namespace Scene2d.Commands
{
    public class RotateSceneCommand : ICommand
    {
        private readonly double angle;

        public RotateSceneCommand(double angle)
        {
            this.angle = angle;
        }

        public void Apply(Scene scene)
        {
            scene.RotateScene(this.angle);
        }
    }
}