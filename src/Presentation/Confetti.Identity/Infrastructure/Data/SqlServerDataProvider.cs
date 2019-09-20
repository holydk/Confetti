using System.Data.Common;
using System.Data.SqlClient;
using Confetti.Account.Infrastructure.Identity.Data;
using IdentityServer4.EntityFramework.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Data
{
    /// <summary>
    /// Represents Identity SQL Server data provider
    /// </summary>
    public class SqlServerDataProvider : Confetti.Account.Infrastructure.Identity.Data.SqlServerDataProvider
    {
        #region Fields

        private readonly PersistedGrantDbContext _persistedContext;
        private readonly ConfigurationDbContext _configurationContext;

        #endregion

        #region Ctor

        public SqlServerDataProvider(
            AppIdentityDbContext identityContext,
            PersistedGrantDbContext persistedContext,
            ConfigurationDbContext configurationContext
        ) : base(identityContext)
        {
            _persistedContext = persistedContext;
            _configurationContext = configurationContext;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public override void InitializeDatabase()
        {     
            // Initialize Identity context
            base.InitializeDatabase(); 

            // Initialize Identity Server 4 contexts     
            _persistedContext.Database.Migrate();
            _configurationContext.Database.Migrate();
        }

        #endregion
    }
}