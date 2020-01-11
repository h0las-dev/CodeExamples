namespace Scene2d.Commands
{
    public class DeleteSceneCommand : ICommand
    { 
        public void Apply(Scene scene)
        {
            scene.DeleteScene();
        }
    }
}