using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Domain.Uow;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Provides an implementation of the unit of work.
    /// </summary>
    public class EfCoreUnitOfWork : IUnitOfWork
    {
        #region Fields

        private bool _isBeginCalledBefore;
        private bool _isCompleteCalledBefore;
        private bool _isDisposed;
        private Dictionary<string, DbContext> _releasableCommandTimeoutDbContexts;    
            
        #endregion

        #region Properties

        protected IConnectionStringResolver ConnectionStringResolver { get; }
        protected IActiveDbContextsService ActiveDbContextsService { get; }
        protected IEfCoreTransactionStrategy EfCoreTransactionStrategy { get; }
        protected UnitOfWorkOptions Options { private set; get; }
            
        #endregion

        #region Ctor

        public EfCoreUnitOfWork(
            IActiveDbContextsService activeDbContextService,
            IEfCoreTransactionStrategy efCoreTransactionStrategy,
            IConnectionStringResolver connectionStringResolver)
        {
            ActiveDbContextsService = activeDbContextService;
            EfCoreTransactionStrategy = efCoreTransactionStrategy;
            ConnectionStringResolver = connectionStringResolver;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Begins a unit of work with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        public virtual void Begin(UnitOfWorkOptions options)
        {
            Options = options ?? throw new ArgumentNullException(nameof(options));

            CheckDisposing();
            PreventMultipleBegin();
            
            ActiveDbContextsService.Clear();
            ActiveDbContextsService.Added += OnAddedDbContext;

            if (Options.IsTransactional == true)
            {
                EfCoreTransactionStrategy.Initialize(options);
            }
        }

        /// <summary>
        /// Saves a changes at the moment.
        /// </summary>
        public virtual async Task SaveChangesAsync()
        {
            CheckDisposing();

            foreach (var context in GetAllActiveDbContexts())
            {
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// Completes a unit of work.
        /// </summary>
        public virtual async Task CompleteAsync()
        {
            CheckDisposing();
            PreventMultipleComplete();
            
            await SaveChangesAsync();
            await CommitAsync();
        }

        public virtual void Dispose()
        {
            if (!_isBeginCalledBefore || _isDisposed) return;

            _isDisposed = true;

            if (Options.IsTransactional == true)
            {
                EfCoreTransactionStrategy.Dispose();
            }

            // if we have not saved entities, detach them. 
            foreach (var context in GetAllActiveDbContexts())
            {
                context.DetachEntities();
            }

            ReleaseCommandTimeouts();

            ActiveDbContextsService.Clear();
            ActiveDbContextsService.Added -= OnAddedDbContext;
        }

        protected virtual async Task CommitAsync()
        {
            if (Options.IsTransactional == true)
            {
                await EfCoreTransactionStrategy.CommitAsync();
            }
        }

        protected virtual IReadOnlyList<DbContext> GetAllActiveDbContexts()
        {
            return ActiveDbContextsService.GetAll().ToImmutableArray();
        }

        private void OnAddedDbContext(DbContext context)
        {
            if (context == null) return;

            CheckDisposing();

            if (Options.Timeout.HasValue 
                  && context.Database.IsRelational()
                     && !context.Database.GetCommandTimeout().HasValue)
            {
                var timeout = Convert.ToInt32(Options.Timeout.Value.TotalSeconds);
                context.Database.SetCommandTimeout(timeout);

                AddReleasableCommandTimeout(context);
            }
        }

        private void AddReleasableCommandTimeout(DbContext context)
        {
            if (_releasableCommandTimeoutDbContexts == null)
                _releasableCommandTimeoutDbContexts = new Dictionary<string, DbContext>();

            var args = new Dictionary<string, object>()
            {
                { "ContextType", context.GetType() }
            };
            var connectionString = ConnectionStringResolver.GetNameOrConnectionString(args);
            var key = $"{connectionString}#{context.GetType().FullName}";

            _releasableCommandTimeoutDbContexts.TryAdd(key, context);
        }

        private void ReleaseCommandTimeouts()
        {
            if (_releasableCommandTimeoutDbContexts == null) return;

            foreach (var dbContext in _releasableCommandTimeoutDbContexts.Values)
            {
                dbContext.Database.SetCommandTimeout(null);
            }

            _releasableCommandTimeoutDbContexts.Clear();
            _releasableCommandTimeoutDbContexts = null;
        }
            
        #endregion

        #region Utilities

        private void PreventMultipleBegin()
        {
            if (_isBeginCalledBefore)
            {
                throw new InvalidOperationException("This unit of work has started before. Can not call Start method more than once.");
            }

            _isBeginCalledBefore = true;
        }

        private void PreventMultipleComplete()
        {
            if (_isCompleteCalledBefore)
            {
                throw new InvalidOperationException("Complete is called before!");
            }

            _isCompleteCalledBefore = true;
        }

        private void CheckDisposing()
        {
            if (_isDisposed)
            {
                throw new ObjectDisposedException("This unit of work is disposed.");
            }
        }
            
        #endregion
    }
}
