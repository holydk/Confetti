using System;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Provides an abstraction for managing of active <see cref="DbContext"/>'s.
    /// </summary>
    public interface IActiveDbContextsService
    {
        /// <summary>
        /// A event that notify when <see cref="DbContext"/> is added.
        /// </summary>
        event Action<DbContext> Added;

        /// <summary>
        /// Gets all <see cref="DbContext"/>'s.
        /// </summary>
        /// <returns>The array of <see cref="DbContext"/>.</returns>
        DbContext[] GetAll();

        /// <summary>
        /// Gets <see cref="TContext"/> if exists.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <returns>The <see cref="TContext"/>.</returns>
        TContext Get<TContext>() where TContext : DbContext;

        /// <summary>
        /// Try to add <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <returns>True if added; otherwise false.</returns>
        bool TryAdd(DbContext context);

        /// <summary>
        /// Clears all <see cref="DbContext"/>'s.
        /// </summary>
        void Clear();
    }
}