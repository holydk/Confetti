using Confetti.Infrastructure.EntityFrameworkCore.SqlServer.Design;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Identity.Infrastructure.Application.IdentityServer4.Data
{
    /// <summary>
    /// Represents a factory to create <see cref="PersistedGrantDbContext" />.
    /// </summary>
    public class PersistedGrantDbContextFactory : SqlServerConfettiContextFactory<PersistedGrantDbContext>
    {
        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="builder">The options builder.</param>
        /// <returns>New instance of <see cref="PersistedGrantDbContext" />.</returns>
        protected override PersistedGrantDbContext CreateDbContext(DbContextOptionsBuilder<PersistedGrantDbContext> builder)
        {
            return new PersistedGrantDbContext(builder.Options, new OperationalStoreOptions());
        }
    }
}