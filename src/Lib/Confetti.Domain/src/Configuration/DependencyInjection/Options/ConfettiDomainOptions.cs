using Confetti.Domain.Uow;

namespace Confetti.Domain.Configuration
{
    /// <summary>
    /// Represents a domain options.
    /// </summary>
    public class ConfettiDomainOptions
    {
        /// <summary>
        /// Gets or sets a default data provider name.
        /// </summary>
        /// <value></value>
        public UnitOfWorkOptions DefaultUnitOfWorkOptions { get; set; } = new UnitOfWorkOptions() { IsTransactional = true };
    }
}