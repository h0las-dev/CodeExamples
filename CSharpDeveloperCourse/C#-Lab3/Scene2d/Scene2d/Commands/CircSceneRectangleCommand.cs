namespace Scene2d.Commands
{
    public class CircSceneRectangleCommand : ICommand
    {
        public void Apply(Scene scene)
        {
            scene.PrintCircumscribingSceneRectangle();
        }
    }
}