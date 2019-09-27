using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore
{
    /// <summary>
    /// Provides an abstraction for a factory to get <see cref="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of database context.</typeparam>
    public interface IDbContextProvider<TContext> where TContext : DbContext
    {
        /// <summary>
        /// Gets a <see cref="TContext"/>.
        /// </summary>
        /// <returns>The <see cref="TContext"/>.</returns>
        TContext Get();
    }
}