using System.Threading.Tasks;

namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides an abstraction for unit of work.
    /// </summary>
    public interface IUnitOfWork : IUnitOfWorkCompleteHandle
    {
        /// <summary>
        /// Begins a unit of work with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        void Begin(UnitOfWorkOptions options);

        /// <summary>
        /// Saves a changes at the moment.
        /// </summary>
        Task SaveChangesAsync();
    }
}