using Confetti.Infrastructure.Data.EntityFrameworkCore.SqlServer;
using Confetti.Infrastructure.FileProviders;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Represents SQL Server data provider
    /// </summary>
    public class SqlServerCatalogDataProvider : SqlServerDataProvider
    {
        #region Fields

        private readonly CatalogContext _context;
        private readonly IConfettiFileProvider _fileProvider;

        #endregion

        #region Ctor

        public SqlServerCatalogDataProvider(
            CatalogContext context,
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
        public override void InitializeDatabase()
        {
            _context.Database.Migrate();

            // create stored procedures 
            _context.ExecuteSqlScriptFromFile(_fileProvider.MapPath(SqlServerCatalogDataDefaults.SqlServerStoredProceduresFilePath));
        }

        #endregion
    }
}