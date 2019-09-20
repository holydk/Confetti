using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Confetti.Domain.Linq
{
    /// <summary>
    /// Provides an abstraction for async execution of <see cref="IQueryable<T>"/>.
    /// </summary>
    public interface IAsyncQueryableExecuter
    {
        /// <summary>
        /// Runs query and return result as <see cref="IList<T>"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        Task<IList<T>> ToListAsync<T>(IQueryable<T> query);

        /// <summary>
        /// Runs query and return result as array.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        Task<T[]> ToArrayAsync<T>(IQueryable<T> query);

        /// <summary>
        /// Runs query and return result as <see cref="IAsyncEnumerable<T>"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        IAsyncEnumerable<T> ToAsyncEnumerable<T>(IQueryable<T> query);
    }
}