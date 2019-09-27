using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Represents the information about active transaction.
    /// </summary>
    public class ActiveTransactionInfo
    {
        /// <summary>
        /// Gets a <see cref="IDbContextTransaction"/>.
        /// </summary>
        /// <value>The <see cref="IDbContextTransaction"/>.</value>
        public IDbContextTransaction DbContextTransaction { get; }

        /// <summary>
        /// Gets a <see cref="DbContext"/> that started transaction.
        /// </summary>
        /// <value></value>
        public DbContext StarterDbContext { get; }

        /// <summary>
        /// Gets a list of <see cref="DbContext"/> that participates in one unit of work. 
        /// </summary>
        /// <value></value>
        public List<DbContext> AttendedDbContexts { get; }

        /// <summary>
        /// Creates a new instance of <see cref="ActiveTransactionInfo"/>.
        /// </summary>
        /// <param name="dbContextTransaction">The <see cref="IDbContextTransaction"/>.</param>
        /// <param name="starterDbContext">The starter <see cref="DbContext"/>.</param>
        public ActiveTransactionInfo(IDbContextTransaction dbContextTransaction, DbContext starterDbContext)
        {
            DbContextTransaction = dbContextTransaction;
            StarterDbContext = starterDbContext;

            AttendedDbContexts = new List<DbContext>();
        }
    }
}