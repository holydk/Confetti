using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Confetti.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Provides an implementation for managing of active <see cref="DbContext"/>'s.
    /// </summary>
    public class ActiveDbContextsService : IActiveDbContextsService
    {
        #region Properties

        protected ConcurrentDictionary<string, DbContext> ActiveDbContexts { get; }
        protected IConnectionStringResolver ConnectionStringResolver { get; }
            
        #endregion

        #region Events

        /// <summary>
        /// A event that notify when <see cref="DbContext"/> is added.
        /// </summary>
        public event Action<DbContext> Added;
            
        #endregion

        #region Ctor

        public ActiveDbContextsService(IConnectionStringResolver connectionStringResolver)
        {
            ActiveDbContexts = new ConcurrentDictionary<string, DbContext>();
            ConnectionStringResolver = connectionStringResolver;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Gets all <see cref="DbContext"/>'s.
        /// </summary>
        /// <returns>The array of <see cref="DbContext"/>.</returns>
        public virtual DbContext[] GetAll()
        {
            return ActiveDbContexts
                .Select(pair => pair.Value)
                .ToArray();
        }

        /// <summary>
        /// Gets <see cref="TContext"/> if exists.
        /// </summary>
        /// <typeparam name="TContext">The context type.</typeparam>
        /// <returns>The <see cref="TContext"/>.</returns>
        public virtual TContext Get<TContext>() where TContext : DbContext
        {
            return (TContext)GetAll()
                .FirstOrDefault(context => context.GetType() == typeof(TContext));
        }

        /// <summary>
        /// Try to add <see cref="DbContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="DbContext"/>.</param>
        /// <returns>True if added; otherwise false.</returns>
        public virtual bool TryAdd(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            var args = new Dictionary<string, object>()
            {
                { "ContextType", context.GetType() }
            };
            var connectionString = ConnectionStringResolver.GetNameOrConnectionString(args);
            var key = $"{connectionString}#{context.GetType().FullName}";

            var isAdded = ActiveDbContexts.TryAdd(key, context);
            if (isAdded)
            {
                Added?.Invoke(context);
            }

            return isAdded;
        }

        /// <summary>
        /// Clears all <see cref="DbContext"/>'s.
        /// </summary>
        public virtual void Clear()
        {
            ActiveDbContexts.Clear();
        }
            
        #endregion
    }
}