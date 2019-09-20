namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.SqlServer
{
    /// <summary>
    /// Represents default values related to Confetti data
    /// </summary>
    public static partial class SqlServerCatalogDataDefaults
    {
        /// <summary>
        /// Gets a path to the file that contains script to create SQL Server stored procedures
        /// </summary>
        public static string SqlServerStoredProceduresFilePath => "~/App_Data/Install/SqlServer.StoredProcedures.sql";
    }
}