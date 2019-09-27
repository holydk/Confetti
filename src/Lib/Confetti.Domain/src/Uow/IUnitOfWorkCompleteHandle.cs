using System;
using System.Threading.Tasks;

namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides an abstraction to complete unit of work.
    /// </summary>
    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        /// <summary>
        /// Completes a unit of work.
        /// </summary>
        Task CompleteAsync();
    }
}