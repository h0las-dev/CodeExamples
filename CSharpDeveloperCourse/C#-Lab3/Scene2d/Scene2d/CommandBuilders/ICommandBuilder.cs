namespace Scene2d
{
    public interface ICommandBuilder
    {
        bool IsCommandReady { get; }

        void AppendLine(string line);

        ICommand GetCommand();
    }
}