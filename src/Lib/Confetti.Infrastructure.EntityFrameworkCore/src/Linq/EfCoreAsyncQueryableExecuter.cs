using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Domain.Linq;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.Linq
{
    /// <summary>
    /// Provides an implementation for EntityFrameworkCore async execution of <see cref="IQueryable{T}"/>.
    /// </summary>
    public class EfCoreAsyncQueryableExecuter : IAsyncQueryableExecuter
    {
        /// <summary>
        /// Runs query and return result as <see cref="IList{T}"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        public Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return query.ToListAsync();
        }

        /// <summary>
        /// Runs query and return result as array.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        public Task<T[]> ToArrayAsync<T>(IQueryable<T> query)
        {
            return query.ToArrayAsync();
        }

        /// <summary>
        /// Runs query and return result as <see cref="IAsyncEnumerable{T}"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        public IAsyncEnumerable<T> ToAsyncEnumerable<T>(IQueryable<T> query)
        {
            return query.AsAsyncEnumerable();
        }

        /// <summary>
        /// Runs query and return result as <see cref="{T}"/>.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <typeparam name="T">The type of the data in the data source.</typeparam>
        /// <returns></returns>
        public Task<T> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            return query.FirstOrDefaultAsync();
        }
    }
}