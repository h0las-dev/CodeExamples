namespace BailManagement.Dapper.DataAccess.Data.GwTransactions.Specs
{
    using Results;
    using BailManagement.Dapper.DataAccess.DataAccess.Queries;

    public class DropboxDefinitionDataSpec : IQuerySpec<DropboxDefinitionDataQr>
    {
        public DropboxDefinitionDataSpec(string checkDefinition)
        {
            CheckDefinition = checkDefinition;
        }

        public string CheckDefinition { get; }
    }
}