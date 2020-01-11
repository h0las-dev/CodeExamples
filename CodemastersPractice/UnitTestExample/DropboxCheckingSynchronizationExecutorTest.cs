namespace BailManagement.Tests.Integration.Executors
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services.DropboxCheckDefinitions;
    using Common.Services.LogMessages;
    using BailManagement.Dapper.DataAccess.Data.GwTransactions.Queries;
    using Queries.Stubs;
    using WinService.Core.Executors;
    using WinService.Services;
    using WinService.Services.DropboxCheckDefinitionData;
    using NUnit.Framework;
    using Domain;

    [TestFixture]
    public class DropboxCheckingSynchronizationExecutorTest : DataAccessTest
    {
        private DropboxSynchronizationHealthCheckServiceStub DropboxSynchronizationHealthCheckService { get; set; }
        private DropboxDefinitionDataManager DropboxDefinitionFileManager { get; set; }
        private EmailServiceStub EmailServiceManager { get; set; }
        private DropboxCheckingSynchronizationExecutor DropboxSynchronizationExecutor { get; set; }

        [SetUp]
        public void CreateServiceStubs()
        {
            DropboxSynchronizationHealthCheckService = new DropboxSynchronizationHealthCheckServiceStub();
            EmailServiceManager = new EmailServiceStub();

            DropboxDefinitionFileManager = new DropboxDefinitionDataManager
            (
                new GetDropboxDefinitionDataQuery(Config.ConnectionString), 
                new UpdateDropboxDefinitionDataQuery(Config.ConnectionString),
                new CreateDropboxDefinitionDataQuery(Config.ConnectionString)
            );

            DropboxSynchronizationExecutor = CreateDropboxExecutor();
        }

        [Test]
        public async Task TestSendEmailIfDropboxSynchronized()
        {
            DropboxSynchronizationHealthCheckService.SynchronizationResult = SynchronizationResult.ResultShouldBeSuccess;

            await DropboxSynchronizationExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(LogMessages.SyncSuccess, EmailServiceManager.LastSendingEmailNotification);
        }

        [Test]
        public async Task TestSendEmailIfDropboxFailed()
        {
            DropboxSynchronizationHealthCheckService.SynchronizationResult = SynchronizationResult.ResultShouldBeFailed;

            await DropboxSynchronizationExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(LogMessages.SyncError, EmailServiceManager.LastSendingEmailNotification);
        }

        [Test]
        public async Task TestSendEmailIfDropboxAlreadySynchronized()
        {
            SetSynchronizationStatusInTestDb(DropboxHealthCheckDataStatus.SyncSuccess);
            DropboxSynchronizationHealthCheckService.SynchronizationResult = SynchronizationResult.ResultShouldBeSuccess;

            await DropboxSynchronizationExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(0, EmailServiceManager.SendingEmailNotifications.Count);
        }

        [Test]
        public async Task TestSendEmailIfDropboxAlreadyFailed()
        {
            SetSynchronizationStatusInTestDb(DropboxHealthCheckDataStatus.SyncFailed);
            DropboxSynchronizationHealthCheckService.SynchronizationResult = SynchronizationResult.ResultShouldBeFailed;

            await DropboxSynchronizationExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(0, EmailServiceManager.SendingEmailNotifications.Count);
        }

        private DropboxCheckingSynchronizationExecutor CreateDropboxExecutor()
        {
            return new DropboxCheckingSynchronizationExecutor
            (
                DropboxSynchronizationHealthCheckService,
                DropboxDefinitionFileManager,
                EmailServiceManager,
                new EmailConstructor(new ConfigDataAccessProviderStub())
            );
        }

        private void SetSynchronizationStatusInTestDb(DropboxHealthCheckDataStatus status)
        {
            var definitionField = DropboxCheckDefinitions.DropboxSynchronizationStatus;
            var definitionTable = "DropboxHealthCheckData";

            InsertAndReturnId(definitionTable, new
            {
                CheckDefinition = definitionField,
                FileName = "TestFile",
                Status = (int)status
            });
        }
    }
}