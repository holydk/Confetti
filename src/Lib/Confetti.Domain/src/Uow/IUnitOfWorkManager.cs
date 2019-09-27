namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides an abstraction for unit of work manager.
    /// </summary>
    public interface IUnitOfWorkManager
    {
        /// <summary>
        /// Begins a unit of work with default <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <returns>The uow complete handle.</returns>
        IUnitOfWorkCompleteHandle Begin();

        /// <summary>
        /// Begins a unit of work with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        /// <returns>The uow complete handle.</returns>
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }
}