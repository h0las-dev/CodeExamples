namespace BailManagement.Common.Services.WinServices
{
    using BailManagement.Common.Domain.WinServices;
    using Domain;
    using System.ServiceProcess;

    public interface IWinServicesManager: IServicesMarker
    {
        WinService[] GetWinServices();

        void ChangeWinServiceStatus(WinService service);

        WinService GetWinServiceByName(string serviceName);

        void SetServiceStatusDirectly(WinService service, DropboxHealthCheckDataStatus status);

        bool TrySetServiceStatus(WinService service, DropboxHealthCheckDataStatus status);

        bool TryReloadService(WinService service);

        ServiceController GetServiceControllerByDisplayName(string serviceDisplayName);

        ServiceController GetServiceControllerByName(string serviceName);
    }
}