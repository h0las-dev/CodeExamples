using Dapper;

namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Queries
{
    using Results;
    using Specs;
    using DataAccess;
    using System.Data.SqlClient;

    public class GetDropboxDefinitionDataQuery : GenericQuery<DropboxDefinitionDataSpec, DropboxDefinitionDataQr>
    {
        public GetDropboxDefinitionDataQuery(string connectionString) : base(connectionString)
        {

        }

        protected override DropboxDefinitionDataQr ExecuteQuery(DropboxDefinitionDataSpec spec, SqlConnection connection)
        {
            var sql = @"select top 1 [FileName], [CreatedAt], [UpdatedAt], [Status], [CheckDefinition] from DropboxHealthCheckData where [CheckDefinition] = @CheckDefinition";

            var data = connection.QueryFirstOrDefault<DropboxDefinitionDataQr>(sql, spec);

            return data;
        }
    }
}