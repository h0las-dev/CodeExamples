namespace Scene2d.Commands
{
    public class CopySceneCommand : ICommand
    {
        private readonly string copyName;

        public CopySceneCommand(string copyName)
        {
            this.copyName = copyName;
        }

        public void Apply(Scene scene)
        {
            scene.CopyScene(this.copyName);
        }
    }
}