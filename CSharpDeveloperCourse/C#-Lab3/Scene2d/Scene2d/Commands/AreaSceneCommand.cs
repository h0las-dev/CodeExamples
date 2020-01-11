namespace Scene2d.Commands
{
    public class AreaSceneCommand : ICommand
    {
        public void Apply(Scene scene)
        {
            scene.PrintSceneArea();
        }
    }
}