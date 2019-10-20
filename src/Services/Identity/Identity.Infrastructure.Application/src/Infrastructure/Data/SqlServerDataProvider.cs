using Confetti.Identity.Infrastructure.Application.AspNetIdentity.Data;
using Confetti.Infrastructure.EntityFrameworkCore.SqlServer;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Application.Data
{
    /// <summary>
    /// Represents a SQL Server data provider handler for Identity service.
    /// </summary>
    public class SqlServerIdentityDataProviderHandler : SqlServerDataProviderHandler
    {
        #region Fields

        private readonly AppIdentityDbContext _identityContext;
        private readonly PersistedGrantDbContext _persistedContext;
        private readonly ConfigurationDbContext _configurationContext;

        #endregion

        #region Ctor

        public SqlServerIdentityDataProviderHandler(
            AppIdentityDbContext identityContext,
            PersistedGrantDbContext persistedContext,
            ConfigurationDbContext configurationContext)
        {
            _identityContext = identityContext;
            _persistedContext = persistedContext;
            _configurationContext = configurationContext;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Initializes a database.
        /// </summary>
        public override void InitializeDatabase()
        {
            // Make sure we have the database       
            _identityContext.Database.Migrate();
            _persistedContext.Database.Migrate();
            _configurationContext.Database.Migrate();
        }

        #endregion
    }
}