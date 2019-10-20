using Confetti.Infrastructure.EntityFrameworkCore.SqlServer.Design;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Application.IdentityServer4.Data
{
    /// <summary>
    /// Represents a factory to create <see cref="ConfigurationDbContext" />.
    /// </summary>
    public class ConfigurationDbContextFactory : SqlServerConfettiContextFactory<ConfigurationDbContext>
    {
        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="builder">The options builder.</param>
        /// <returns>New instance of <see cref="ConfigurationDbContext" />.</returns>
        protected override ConfigurationDbContext CreateDbContext(DbContextOptionsBuilder<ConfigurationDbContext> builder)
        {
            return new ConfigurationDbContext(builder.Options, new ConfigurationStoreOptions());
        }
    }
}