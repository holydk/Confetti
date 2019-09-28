using System.Data.Common;
using System.Data.SqlClient;
using Confetti.Data.Services;

namespace Confetti.Infrastructure.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Represents an abstraction for handler of the sql server data provider.
    /// </summary>
    public abstract class SqlServerDataProviderHandler : IDataProviderHandler
    {
        #region Methods

        /// <summary>
        /// Initializes a database.
        /// </summary>
        public abstract void InitializeDatabase();

        /// <summary>
        /// Gets a support database parameter object (used by stored procedures).
        /// </summary>
        /// <returns>The database parameter.</returns>
        public virtual DbParameter GetParameter()
        {
            return new SqlParameter();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether this data provider supports backup.
        /// </summary>
        public virtual bool BackupSupported => true;

        /// <summary>
        /// Gets a maximum length of the data for HASHBYTES functions, returns 0 if HASHBYTES function is not supported.
        /// </summary>
        public virtual int SupportedLengthOfBinaryHash => 8000; // for SQL Server 2008 and above HASHBYTES function has a limit of 8000 characters.

        #endregion
    }
}