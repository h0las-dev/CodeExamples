namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Queries
{
    using System.Data.SqlClient;
    using DataAccess;
    using global::Dapper;
    using Specs;

    public class CreateDropboxDefinitionDataQuery : GenericQuery<UpdateDropboxDefinitionDataSpec, bool>
    {
        public CreateDropboxDefinitionDataQuery(string connectionString) : base(connectionString)
        {
        }

        protected override bool ExecuteQuery(UpdateDropboxDefinitionDataSpec spec, SqlConnection connection)
        {
            //var format = "yyyy-MM-dd HH:mm:ss";

            return connection.Execute("insert DropboxHealthCheckData (FileName, CreatedAt, UpdatedAt, Status, CheckDefinition) values (@FileName, @CreatedAt, @UpdatedAt, @Status, @CheckDefinition)", spec) != 0;
        }
    }
}