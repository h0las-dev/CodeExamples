namespace BailManagement.WinService.Core.Executors.Factories
{
    using Common.Services;
    using Services;
    public interface IExecutorFactory: IServicesMarker
    {
        string GetCode();
        
        IJobExecutor GetExecutor();
    }
}