namespace BailManagement.WinService.Services.DropboxSynchronizationHealthCheck
{
    using Common.Domain;

    using System.Threading.Tasks;

    using BailManagement.Common.Services;

    public interface IDropboxSynchronizationHealthCheckService : IServicesMarker
    {
        Task<DropboxHealthCheckDataStatus> CheckSynchronizationAsync(string testFile);
    }
}
