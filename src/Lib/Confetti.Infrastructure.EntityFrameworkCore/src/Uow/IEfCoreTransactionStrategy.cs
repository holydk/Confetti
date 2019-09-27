using System;
using System.Threading.Tasks;
using Confetti.Domain.Uow;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Provides an abstraction for the entity framework core transaction strategy.
    /// </summary>
    public interface IEfCoreTransactionStrategy : IDisposable
    {
        /// <summary>
        /// Initializes a transaction strategy with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        void Initialize(UnitOfWorkOptions options);

        /// <summary>
        /// Commits a transaction strategy.
        /// </summary>
        Task CommitAsync();
    }
}