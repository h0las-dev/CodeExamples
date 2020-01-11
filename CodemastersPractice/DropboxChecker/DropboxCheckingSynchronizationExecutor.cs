namespace BailManagement.WinService.Core.Executors
{
    using Common.Services.DropboxCheckDefinitions;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Services.DropboxCheckDefinitionData;
    using Services.DropboxSynchronizationHealthCheck;
    using Services;
    using Common.Domain;
    using Common.Domain.Emails;
    using BailManagement.Common.Services;
    using BailManagement.Common.Services.LogMessages;

    public class DropboxCheckingSynchronizationExecutor : IJobExecutor
    {
        private readonly IDropboxSynchronizationHealthCheckService _dropboxSynchronizationHealthCheckService;
        private readonly IDropboxDefinitionDataManager _dropboxDefinitionDataManager;
        private readonly IEmailService _emailServiceManager;
        private readonly IEmailConstructor _emailConstructor;

        public DropboxCheckingSynchronizationExecutor(IDropboxSynchronizationHealthCheckService dropboxSynchronizationHealthCheckService, IDropboxDefinitionDataManager dropboxDefinitionDataManager,
            IEmailService emailServiceManager, IEmailConstructor emailConstructor)
        {
            _dropboxSynchronizationHealthCheckService = dropboxSynchronizationHealthCheckService;
            _dropboxDefinitionDataManager = dropboxDefinitionDataManager;
            _emailServiceManager = emailServiceManager;
            _emailConstructor = emailConstructor;
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var currentSyncFile = _dropboxDefinitionDataManager.GetData(DropboxCheckDefinitions.DropboxSynchronizationStatus);

            if (currentSyncFile == null)
            {
                CreateDropboxDataDefinition(null, DateTime.Now, DateTime.Now, DropboxHealthCheckDataStatus.SyncUndefined);
            }

            await CheckSynchronization();
        }

        private async Task CheckSynchronization()
        {
            var testFileName = ("TestFile(" + DateTime.Now + ")").Replace(':', '-'); ;

            SyncStatusHandler(testFileName, await _dropboxSynchronizationHealthCheckService.CheckSynchronizationAsync(testFileName));
        }

        private void SyncStatusHandler(string testFileName, DropboxHealthCheckDataStatus currentStatus)
        {
            var notificationMessage = string.Empty;
            var notificationSubject = string.Empty;
            var alreadySendNotificationMessage = string.Empty;

            switch (currentStatus)
            {
                case DropboxHealthCheckDataStatus.SyncSuccess:
                    notificationMessage = LogMessages.SyncSuccess;
                    notificationSubject = LogMessages.SuccessSubject;
                    alreadySendNotificationMessage = LogMessages.SyncAlreadyRunning;
                    break;
                case DropboxHealthCheckDataStatus.SyncFailed:
                    notificationMessage = LogMessages.SyncError;
                    notificationSubject = LogMessages.ErrorSubject;
                    alreadySendNotificationMessage = LogMessages.SyncAlreadyStopped;
                    break;
            }

            if (DropboxStatusIsChanged(currentStatus))
            {
                SetDropboxDefinitionData(testFileName, DateTime.Now, currentStatus);
                Logger.Log.Info(notificationMessage);
                SendNotification(notificationMessage, notificationSubject);
            }
            else
            {
                SetDropboxDefinitionData(testFileName, DateTime.Now, currentStatus);
                Logger.Log.Info(alreadySendNotificationMessage);
            }
        }

        private void SetDropboxDefinitionData(string fileName, DateTime updatedAt, DropboxHealthCheckDataStatus status)
        {
            _dropboxDefinitionDataManager.UpdateData(fileName, updatedAt, status, DropboxCheckDefinitions.DropboxSynchronizationStatus);
        }

        private EmailToSendModel CreateMailModel(string body, string subject)
        {
            return _emailConstructor.GetEmailModel(body, subject);
        }

        private DropboxHealthCheckDataStatus GetSynchronizationStatus()
        {
            return _dropboxDefinitionDataManager.GetData(DropboxCheckDefinitions.DropboxSynchronizationStatus).Status;
        }

        private bool CreateDropboxDataDefinition(string fileName, DateTime createdAt, DateTime updatedAt, DropboxHealthCheckDataStatus status)
        {
            return _dropboxDefinitionDataManager.CreateData(fileName, createdAt, updatedAt, status, DropboxCheckDefinitions.DropboxSynchronizationStatus);
        }

        private bool DropboxStatusIsChanged(DropboxHealthCheckDataStatus currentStatus)
        {
            var oldStatus = GetSynchronizationStatus();
            return currentStatus != oldStatus;
        }

        private void SendNotification(string message, string subject)
        {
            _emailServiceManager.Send(CreateMailModel(message, subject));
        }
    }
}