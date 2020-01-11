using Dapper;

namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Queries
{
    using DataAccess;
    using Specs;
    using System.Data.SqlClient;

    public class UpdateDropboxDefinitionDataQuery : GenericQuery<UpdateDropboxDefinitionDataSpec, bool>
    {
        public UpdateDropboxDefinitionDataQuery(string connectionString) : base(connectionString)
        {

        }

        protected override bool ExecuteQuery(UpdateDropboxDefinitionDataSpec spec, SqlConnection connection)
        {
            //var format = "yyyy-MM-dd HH:mm:ss";

            return connection.Execute("update DropboxHealthCheckData set [FileName] = @FileName, [UpdatedAt] = @UpdatedAt, [Status] = @Status where [CheckDefinition] = @CheckDefinition", spec) != 0;
        }
    }
}