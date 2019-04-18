using System.Data.Common;
using System.Data.SqlClient;
using Confetti.Common.Data;
using Confetti.Infrastructure.Data.Extensions;
using Confetti.Infrastructure.FileProviders;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Data
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public partial class SqlServerDataProvider : IDataProvider
    {
        #region Fields

        private readonly DbContext _context;
        private readonly IConfettiFileProvider _fileProvider;

        #endregion

        #region Ctor

        public SqlServerDataProvider(
            DbContext context,
            IConfettiFileProvider fileProvider)
        {
            _context = context;
            _fileProvider = fileProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize database
        /// </summary>
        public virtual void InitializeDatabase()
        {     
            // Make sure we have the database       
            _context.Database.Migrate();

            // create stored procedures 
            _context.ExecuteSqlScriptFromFile(_fileProvider.MapPath(ConfettiDataDefaults.SqlServerStoredProceduresFilePath));
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