namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Results
{
    using System;

    using Common.Domain;

    public class DropboxDefinitionDataQr
    {
        public string FileName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public DropboxHealthCheckDataStatus Status { get; set; }
        public string CheckDefinition { get; set; }
    }
}