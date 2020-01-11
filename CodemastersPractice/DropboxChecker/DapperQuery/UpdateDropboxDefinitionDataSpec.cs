namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Specs
{
    using System;
    using DataAccess.Queries;
    using Common.Domain;

    public class UpdateDropboxDefinitionDataSpec : IQuerySpec<bool>
    {
        public UpdateDropboxDefinitionDataSpec(string fileName, DateTime createdAt, DateTime updatedAt, string checkDefinition, DropboxHealthCheckDataStatus status)
        {
            FileName = fileName;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            Status = status;
            CheckDefinition = checkDefinition;
        }

        public string FileName { get; }
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
        public DropboxHealthCheckDataStatus Status { get; }
        public string CheckDefinition { get; }
    }
}