using Microsoft.EntityFrameworkCore;


namespace FlashCard.Database
{
    public class CosmosDbContextBase: DbContext 
    {
        protected String _AccountEndpoint;
        protected String _AuthorizationKey;
        protected String _DatabaseName;
        protected String _ContainerName;


        public CosmosDbContextBase(String AccountEndpoint, String AuthorizationKey, String DatabaseName, String ContainerName)
        {
            AccountEndpoint = (AccountEndpoint ?? "").Trim();
            if (String.IsNullOrEmpty(AccountEndpoint)) { throw new ArgumentException($"The parameter '{nameof(AccountEndpoint)}' was null, empty, blank, or otherwise invalid."); }

            AuthorizationKey = (AuthorizationKey ?? "").Trim();
            if (String.IsNullOrEmpty(AuthorizationKey)) { throw new ArgumentException($"The parameter '{nameof(AuthorizationKey)}' was null, empty, blank, or otherwise invalid."); }

            DatabaseName = (DatabaseName ?? "").Trim();
            if (String.IsNullOrEmpty(DatabaseName)) { throw new ArgumentException($"The parameter '{nameof(DatabaseName)}' was null, empty, blank, or otherwise invalid."); }

            ContainerName = (ContainerName ?? "").Trim();
            if (String.IsNullOrEmpty(ContainerName)) { throw new ArgumentException($"The parameter '{nameof(ContainerName)}' was null, empty, blank, or otherwise invalid."); }


            this._AccountEndpoint = AccountEndpoint;
            this._AuthorizationKey = AuthorizationKey;
            this._DatabaseName = DatabaseName;
            this._ContainerName = ContainerName;
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseCosmos(this._AccountEndpoint, this._AuthorizationKey, databaseName: this._DatabaseName);

    }
}
