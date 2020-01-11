namespace BailManagement.WinService.Core.Executors.Factories
{
    using Common.Services;

    public class DropboxCheckingSynchronizationFactory : IExecutorFactory
    {
        private readonly IResolver _resolver;

        public DropboxCheckingSynchronizationFactory(IResolver resolver)
        {
            _resolver = resolver;
        }

        public string GetCode()
        {
            return "CheckDropboxSynchronization";
        }

        public IJobExecutor GetExecutor()
        {
            return _resolver.Get<DropboxCheckingSynchronizationExecutor>();
        }
    }
}