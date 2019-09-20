using Confetti.Infrastructure.Data.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore.SqlServer.Design
{
    /// <summary>
    /// Represents a abstractions for sql server database context factory.
    /// Note: The database context should contain the constructor with DbContextOptions.
    /// </summary>
    /// <typeparam name="TContext">The type of database context.</typeparam>
    public abstract class SqlServerConfettiContextFactory<TContext> : ConfettiContextFactory<TContext> where TContext: DbContext
    {
        #region Properties

        /// <summary>
        /// Get field name of connection string contains in <see cref="ConfettiContextFactory.DataSettingsFileName" /> "ConnectionStrings" section.
        /// Default value is DefaultConnection.
        /// </summary>
        protected virtual string ConnectionStringFieldName => "DefaultConnection";

        /// <summary>
        /// Get migrations assembly field name contains in <see cref="ConfettiContextFactory.DataSettingsFileName" />.
        /// Default value is MigrationsAssemblyName.
        /// <remarks>
        /// If migrations assembly field name is not found than migrations assembly is current assembly.
        /// </remarks>
        /// </summary>
        protected virtual string MigrationsAssemblyFieldName => "MigrationsAssemblyName";
            
        #endregion

        #region Methods

        /// <summary>
        /// Configures the context options from configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="builder">The options builder.</param>
        protected override void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder<TContext> builder)
        {
            base.ConfigureDbContextOptions(configuration, builder);

            var connectionString = configuration.GetConnectionString(ConnectionStringFieldName);
            var migrationsAssemblyName = configuration.GetValue<string>(MigrationsAssemblyFieldName, null);
            builder.UseSqlServer(connectionString, b => b.MigrationsAssembly(migrationsAssemblyName));
        }
            
        #endregion
    }
}