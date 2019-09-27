using Confetti.Infrastructure.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore
{
    /// <summary>
    /// Provides an implementation for a factory to get <see cref="TContext"/>.
    /// </summary>
    /// <typeparam name="TContext">The type of database context.</typeparam>
    public class DbContextProvider<TContext> : IDbContextProvider<TContext> where TContext : DbContext
    {
        #region Properties

        protected TContext Context { get; }
        protected IActiveDbContextsService ActiveDbContextService { get; }
            
        #endregion

        #region Ctor

        public DbContextProvider(
            TContext context, 
            IActiveDbContextsService activeDbContextService)
        {
            Context = context;
            ActiveDbContextService = activeDbContextService;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Gets a <see cref="TContext"/>.
        /// </summary>
        /// <returns>The <see cref="TContext"/>.</returns>
        public virtual TContext Get()
        {
            ActiveDbContextService.TryAdd(Context);
            return Context;
        }
            
        #endregion
    }
}