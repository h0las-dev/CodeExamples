namespace BailManagement.WinService.Services.DropboxCheckDefinitionData
{
    using System;
    using BailManagement.Common.Services;
    using Dapper.DataAccess.Data.GwTransactions.Results;
    using Common.Domain;

    public interface IDropboxDefinitionDataManager : IServicesMarker
    {
        DropboxDefinitionDataQr GetData(string definition);

        bool UpdateData(string fileName, DateTime updatedAt, DropboxHealthCheckDataStatus status, string definition);
        
        bool CreateData(string fileName, DateTime createdAt, DateTime updatedAt, DropboxHealthCheckDataStatus status, string definition);
    }
}