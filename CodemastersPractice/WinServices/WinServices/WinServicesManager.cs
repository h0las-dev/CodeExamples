namespace BailManagement.Common.Services.WinServices
{
    using System;
    using System.Configuration;
    using System.ServiceProcess;
    using System.Linq;
    using BailManagement.Common.Domain.WinServices;
    using Domain;

    public class WinServicesManager : IWinServicesManager
    {
        public WinService[] GetWinServices()
        {
            return ServiceController.GetServices().Select(x => new WinService(x.DisplayName, MapServiceStatus(x.Status))).ToArray();
        }

        public void ChangeWinServiceStatus(WinService service)
        {
            SetNewStatus(service);
        }

        public ServiceController GetServiceControllerByDisplayName(string serviceDisplayName)
        {
            return ServiceController.GetServices().FirstOrDefault(x => x.DisplayName == serviceDisplayName);
        }

        public ServiceController GetServiceControllerByName(string serviceName)
        {
            return ServiceController.GetServices().FirstOrDefault(x => x.ServiceName == serviceName);
        }

        public WinService GetWinServiceByName(string serviceName)
        {
            var serviceController = GetServiceControllerByDisplayName(serviceName);

            return new WinService(serviceController.DisplayName, MapServiceStatus(serviceController.Status));
        }

        public void SetServiceStatusDirectly(WinService service, DropboxHealthCheckDataStatus status)
        {
            var serviceController = GetServiceControllerByDisplayName(service.Name);
            var timeForWaiting = TimeSpan.Parse(ConfigurationManager.AppSettings["WinServicesManager.DelayForCheckingWinServiceStatus"]);

            switch (status)
            {
                case DropboxHealthCheckDataStatus.WinStatusRunning:
                    serviceController.Start();
                    serviceController.WaitForStatus(ServiceControllerStatus.Running, timeForWaiting);
                    service.StatusId = DropboxHealthCheckDataStatus.WinStatusRunning;
                    break;
                case DropboxHealthCheckDataStatus.WinStatusStopped:
                    serviceController.Stop();
                    serviceController.WaitForStatus(ServiceControllerStatus.Stopped, timeForWaiting);
                    service.StatusId = DropboxHealthCheckDataStatus.WinStatusStopped;
                    break;
            }
        }

        public bool TrySetServiceStatus(WinService service, DropboxHealthCheckDataStatus status)
        {
            var serviceController = GetServiceControllerByDisplayName(service.Name);

            if (MapServiceStatus(serviceController.Status) == status)
            {
                return true;
            }

            try
            { 
                SetServiceStatusDirectly(service, status);
            }
            catch (Exception ex)
            {
                Console.WriteLine("error: can't reload a service: " + ex.Message);
                return false;
            }

            return service.StatusId == status;
        }

        public bool TryReloadService(WinService service)
        {
            var isServiceStopped = TrySetServiceStatus(service, DropboxHealthCheckDataStatus.WinStatusStopped);
            var isServiceStarted = false;

            if (isServiceStopped)
            {
                isServiceStarted = TrySetServiceStatus(service, DropboxHealthCheckDataStatus.WinStatusRunning);
            }

            if (isServiceStopped && isServiceStarted)
            {
                return true;
            }

            Console.WriteLine("error: can't reload a service: ");

            return false;
        }

        private void SetNewStatus(WinService service)
        {
            switch (service.StatusId)
            {
                case DropboxHealthCheckDataStatus.WinStatusRunning:
                    service.StatusId = DropboxHealthCheckDataStatus.WinStatusRunning;
                    TrySetServiceStatus(service, DropboxHealthCheckDataStatus.WinStatusRunning);
                    break;
                case DropboxHealthCheckDataStatus.WinStatusStopped:
                    service.StatusId = DropboxHealthCheckDataStatus.WinStatusStopped;
                    TrySetServiceStatus(service, DropboxHealthCheckDataStatus.WinStatusStopped);
                    break;
                case DropboxHealthCheckDataStatus.WinStatusRestart:
                    TryReloadService(service);
                    break;
                default:
                    throw new Exception("incorrect statusId");
            }
        }

        private DropboxHealthCheckDataStatus MapServiceStatus(ServiceControllerStatus status)
        {
            switch (status)
            {
                case ServiceControllerStatus.Running:
                    return DropboxHealthCheckDataStatus.WinStatusRunning;
                case ServiceControllerStatus.Stopped:
                    return DropboxHealthCheckDataStatus.WinStatusStopped;
                default:
                    throw new Exception("incorrect status");
            }
        }
    }
}