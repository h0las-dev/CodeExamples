namespace BailManagement.Common.Domain.WinServices
{
    public class WinService
    {
        public string Name { get; set; }

        public DropboxHealthCheckDataStatus StatusId { get; set; }

        public WinService(string name, DropboxHealthCheckDataStatus id)
        {
            Name = name;
            StatusId = id;
        }
    }
}