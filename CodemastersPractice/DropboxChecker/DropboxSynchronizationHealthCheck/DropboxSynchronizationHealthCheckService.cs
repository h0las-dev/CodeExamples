namespace BailManagement.WinService.Services.DropboxSynchronizationHealthCheck
{
    using System;
    using System.IO;
    using FileSystemServices;
    using System.Threading.Tasks;
    using Common.Domain;
    using BailManagement.Common.Services.ConfigDataAccessProvider;

    public class DropboxSynchronizationHealthCheckService : IDropboxSynchronizationHealthCheckService
    {
        private readonly IDropboxSyncFileSystemService _dropboxFileSyncSystemService;
        private readonly IFileSyncSystemService _fileSyncSystemService;
        private readonly IConfigDataAccessProvider _configDataAccessProvider;

        public DropboxSynchronizationHealthCheckService(IDropboxSyncFileSystemService dropboxFileSyncSystemService, IFileSyncSystemService fileSyncSystemService,
            IConfigDataAccessProvider configDataAccessProvider)
        {
            _dropboxFileSyncSystemService = dropboxFileSyncSystemService;
            _fileSyncSystemService = fileSyncSystemService;
            _configDataAccessProvider = configDataAccessProvider;
        }

        public async Task<DropboxHealthCheckDataStatus> CheckSynchronizationAsync(string testFile)
        {
            RemoveOldTestFiles();

            CreateTestFileOnLocalServer(testFile);

            var syncDelay = int.Parse(_configDataAccessProvider.GetValueByKey("DropboxSynchronizationDelay"));
            await Task.Delay(syncDelay);

            if (await IsTestFileExistOnDropboxAsync(testFile))
            {
                return DropboxHealthCheckDataStatus.SyncSuccess;
            }

            return DropboxHealthCheckDataStatus.SyncFailed;
        }

        private async Task<bool> IsTestFileExistOnDropboxAsync(string fileName)
        {
            var files = await _dropboxFileSyncSystemService.GetFilesAsync();

            return Array.Exists(files, element => element.FileName == fileName);
        }

        private void CreateTestFileOnLocalServer(string fileName)
        {
            var stream = new MemoryStream();

            _fileSyncSystemService.CreateFile(stream, fileName);
        }

        private void RemoveOldTestFiles()
        {
            var files = _fileSyncSystemService.GetFiles();

            foreach (var file in files)
            {
                _fileSyncSystemService.RemoveFile(file.FileName);                
            }
        }
    }
}