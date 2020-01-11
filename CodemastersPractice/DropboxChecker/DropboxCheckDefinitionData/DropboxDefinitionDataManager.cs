namespace BailManagement.WinService.Services.DropboxCheckDefinitionData
{
    using System;
    using Dapper.DataAccess.Data.GwTransactions.Results;
    using Dapper.DataAccess.Data.GwTransactions.Queries;
    using Dapper.DataAccess.Data.GwTransactions.Specs;
    using Common.Domain;

    public class DropboxDefinitionDataManager : IDropboxDefinitionDataManager
    {
        private readonly GetDropboxDefinitionDataQuery _getDropboxSynchronizationFileNameQuery;

        private readonly UpdateDropboxDefinitionDataQuery _updateDropboxSynchronizationFileNameQuery;

        private readonly CreateDropboxDefinitionDataQuery _createDropboxSynchronizationFileNameQuery;

        public DropboxDefinitionDataManager(GetDropboxDefinitionDataQuery getDropboxSynchronizationFileQuery, UpdateDropboxDefinitionDataQuery updateDropboxSynchronizationFileQuery, CreateDropboxDefinitionDataQuery createDropboxSynchronizationFileQuery)
        {
            _getDropboxSynchronizationFileNameQuery = getDropboxSynchronizationFileQuery;
            _updateDropboxSynchronizationFileNameQuery = updateDropboxSynchronizationFileQuery;
            _createDropboxSynchronizationFileNameQuery = createDropboxSynchronizationFileQuery;
        }
        
        public DropboxDefinitionDataQr GetData(string definition)
        {
            return _getDropboxSynchronizationFileNameQuery.ExecuteQuery(new DropboxDefinitionDataSpec(definition));
        }

        public bool UpdateData(string fileName, DateTime updatedAt, DropboxHealthCheckDataStatus status, string definition)
        {
            return _updateDropboxSynchronizationFileNameQuery.ExecuteQuery(new UpdateDropboxDefinitionDataSpec(fileName, updatedAt, updatedAt, definition, status));
        }

        public bool CreateData(string fileName, DateTime createdAt, DateTime updatedAt, DropboxHealthCheckDataStatus status, string definition)
        {
            return _createDropboxSynchronizationFileNameQuery.ExecuteQuery(new UpdateDropboxDefinitionDataSpec(fileName, createdAt, updatedAt, definition, status));
        }
    }
}