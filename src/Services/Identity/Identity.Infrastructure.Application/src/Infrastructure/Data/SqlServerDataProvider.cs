using System.Data.Common;
using System.Data.SqlClient;
using Confetti.Data;
using Confetti.Identity.Infrastructure.Application.AspNetIdentity.Data;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Application.Data
{
    /// <summary>
    /// Represents a SQL Server data provider for Identity context.
    /// to do: create project Confetti.Data.SqlServer and move base sqlserver data provider.
    /// </summary>
    public class SqlServerDataProvider : IDataProvider
    {
        #region Fields

        private readonly AppIdentityDbContext _identityContext;

        #endregion

        #region Ctor

        public SqlServerDataProvider(
            AppIdentityDbContext identityContext
        )
        {
            _identityContext = identityContext;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {     
            // Make sure we have the database       
            _identityContext.Database.Migrate();
        }

        /// <summary>
        /// Get a support database parameter object (used by stored procedures)
        /// </summary>
        /// <returns>Parameter</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup
        /// </summary>
        public virtual bool BackupSupported => true;

        /// <summary>
        /// Gets a maximum length of the data for HASHBYTES functions, returns 0 if HASHBYTES function is not supported
        /// </summary>
        public virtual int SupportedLengthOfBinaryHash => 8000; // for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.
            
        #endregion
    }
}