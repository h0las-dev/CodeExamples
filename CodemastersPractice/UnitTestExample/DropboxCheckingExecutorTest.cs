namespace BailManagement.Tests.Integration.Executors
{
    using System;
    using Common.Services.DropboxCheckDefinitions;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Domain;
    using Common.Services.LogMessages;
    using BailManagement.Dapper.DataAccess.Data.GwTransactions.Queries;
    using WinService.Services;
    using WinService.Services.DropboxCheckDefinitionData;
    using NUnit.Framework;
    using WinService.Core.Executors;
    using Queries.Stubs;

    [TestFixture]
    public class DropboxCheckingExecutorTest : DataAccessTest
    {
        private WinServicesManagerStub WinServicesManager { get; set; }
        private ConfigDataAccessProviderStub ConfigDataAccessProvider { get; set; }
        private EmailServiceStub EmailService { get; set; }
        private DropboxDefinitionDataManager DropboxDefinitionDataManager { get; set; }
        private DropboxCheckingExecutor DropboxExecutor { get; set; }

        [SetUp]
        public void CreateServiceStubs()
        {
            WinServicesManager = new WinServicesManagerStub();
            ConfigDataAccessProvider = new ConfigDataAccessProviderStub();
            EmailService = new EmailServiceStub();

            DropboxDefinitionDataManager = new DropboxDefinitionDataManager
            (
                new GetDropboxDefinitionDataQuery(Config.ConnectionString),
                new UpdateDropboxDefinitionDataQuery(Config.ConnectionString), 
                new CreateDropboxDefinitionDataQuery(Config.ConnectionString)
            );

            DropboxExecutor = CreateDropboxExecutor();
        }

        [Test]
        public async Task TestSendEmailIfServiceWasStoppedButDefinitionIsOk()
        {
            AddDropboxServiceForTest(DropboxHealthCheckDataStatus.WinStatusStopped, DropboxHealthCheckDataStatus.WinStatusRunning);

            await DropboxExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(LogMessages.RunningSuccessAfterFailed, EmailService.LastSendingEmailNotification);
        }

        [Test]
        public async Task TestShouldNotSendEmailIfServiceAlreadyFailed()
        {
            AddDropboxServiceForTest(DropboxHealthCheckDataStatus.WinStatusDisable, DropboxHealthCheckDataStatus.WinStatusDisable);

            await DropboxExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(0, EmailService.SendingEmailNotifications.Count);
        }

        [Test]
        public async Task TestSendEmailIfServiceFailed()
        {
            AddDropboxServiceForTest(DropboxHealthCheckDataStatus.WinStatusDisable, DropboxHealthCheckDataStatus.WinStatusRunning);

            await DropboxExecutor.RunAsync(new CancellationToken());
            Assert.AreEqual(LogMessages.RunningError, EmailService.LastSendingEmailNotification);
        }

        [Test]
        public async Task TestShouldNotSendEmailIfServiceAlreadyRunning()
        {
            AddDropboxServiceForTest(DropboxHealthCheckDataStatus.WinStatusRunning, DropboxHealthCheckDataStatus.WinStatusRunning);

            await DropboxExecutor.RunAsync(new CancellationToken());

            Assert.AreEqual(0, EmailService.SendingEmailNotifications.Count);
        }

        [Test]
        public async Task TestSendEmailIfServiceRestored()
        {
            AddDropboxServiceForTest(DropboxHealthCheckDataStatus.WinStatusStopped, DropboxHealthCheckDataStatus.WinStatusStopped);
            var dropboxService = WinServicesManager.GetWinServiceByName(ConfigDataAccessProvider.GetValueByKey(AppConfigKeys.DropboxServiceName));

            Assert.AreEqual(DropboxHealthCheckDataStatus.WinStatusStopped, dropboxService.StatusId);

            await DropboxExecutor.RunAsync(new CancellationToken());

            dropboxService = WinServicesManager.GetWinServiceByName(ConfigDataAccessProvider.GetValueByKey(AppConfigKeys.DropboxServiceName));

            Assert.AreEqual(DropboxHealthCheckDataStatus.WinStatusRunning, dropboxService.StatusId);
            Assert.AreEqual(LogMessages.RunningSuccess, EmailService.LastSendingEmailNotification);
        }
        
        private DropboxCheckingExecutor CreateDropboxExecutor()
        {
            return new DropboxCheckingExecutor
            (
                WinServicesManager,
                EmailService,
                new EmailConstructor(new ConfigDataAccessProviderStub()),
                DropboxDefinitionDataManager,
                ConfigDataAccessProvider
            );
        }

        private void AddDropboxServiceForTest(DropboxHealthCheckDataStatus status, DropboxHealthCheckDataStatus definitionStatus)
        {
            WinServicesManager.AddOrUpdateWinService(ConfigDataAccessProvider.GetValueByKey(AppConfigKeys.DropboxServiceName), status);
            DropboxDefinitionDataManager.CreateData(null, DateTime.Now, DateTime.Now, definitionStatus, DropboxCheckDefinitions.DropboxServiceStatus);
        }
    }
}