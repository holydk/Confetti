using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Transactions;
using Confetti.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Confetti.Infrastructure.EntityFrameworkCore.Uow
{
    /// <summary>
    /// Provides an implementation for the EntityFrameworkCore transaction strategy.
    /// </summary>
    public class DbContextEfCoreTransactionStrategy : IEfCoreTransactionStrategy
    {
        #region Fields

        private bool _isInitializeCalledBefore;
        private bool _isDisposed;
            
        #endregion

        #region Properties

        protected IActiveDbContextsService ActiveDbContextService { get; }
        protected IConnectionStringResolver ConnectionStringResolver { get; }
        protected UnitOfWorkOptions Options { private set; get; }
        protected IDictionary<string, ActiveTransactionInfo> ActiveTransactions { get; }
            
        #endregion

        #region Ctor

        public DbContextEfCoreTransactionStrategy(
            IActiveDbContextsService activeDbContextService,
            IConnectionStringResolver connectionStringResolver)
        {
            ActiveDbContextService = activeDbContextService;
            ConnectionStringResolver = connectionStringResolver;

            ActiveTransactions = new Dictionary<string, ActiveTransactionInfo>();
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Initializes a transaction strategy with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        public void Initialize(UnitOfWorkOptions options)
        {
            CheckDisposing();
            PreventMultipleInitialize();
            
            Options = options ?? throw new System.ArgumentNullException(nameof(options));
            ActiveDbContextService.Added += OnAddedDbContext;
        }

        /// <summary>
        /// Commits a transaction strategy.
        /// </summary>
        public async Task CommitAsync()
        {
            CheckDisposing();

            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                await activeTransaction.DbContextTransaction.CommitAsync();

                foreach (var dbContext in activeTransaction.AttendedDbContexts)
                {
                    if (dbContext.HasRelationalTransactionManager())
                    {
                        //Relational databases use the shared transaction
                        continue;
                    }

                    dbContext.Database.CommitTransaction();
                }
            }
        }

        public virtual void Dispose()
        {
            if (!_isInitializeCalledBefore || _isDisposed) return;

            _isDisposed = true;

            foreach (var activeTransaction in ActiveTransactions.Values)
            {
                activeTransaction.DbContextTransaction.Dispose();
            }

            ActiveTransactions.Clear();
            ActiveDbContextService.Added -= OnAddedDbContext;
        }

        private async void OnAddedDbContext(DbContext context)
        {
            if (!Options.IsTransactional.HasValue 
                  || Options.IsTransactional == false
                    || context == null) return;

            CheckDisposing();

            var args = new Dictionary<string, object>()
            {
                { "ContextType", context.GetType() }
            };
            var connectionString = ConnectionStringResolver.GetNameOrConnectionString(args);
            ActiveTransactions.TryGetValue(connectionString, out var activeTransaction);

            if (activeTransaction == null)
            {
                var isolationLevel = (Options.IsolationLevel ?? IsolationLevel.ReadUncommitted).ToSystemDataIsolationLevel();
                var dbtransaction = await context.Database.BeginTransactionAsync(isolationLevel);
                activeTransaction = new ActiveTransactionInfo(dbtransaction, context);
                ActiveTransactions[connectionString] = activeTransaction;
            }
            else
            {
                if (context.HasRelationalTransactionManager())
                {
                    await context.Database.UseTransactionAsync(activeTransaction.DbContextTransaction.GetDbTransaction());
                }
                else
                {
                    await context.Database.BeginTransactionAsync();
                }

                activeTransaction.AttendedDbContexts.Add(context);
            }
        }
            
        #endregion

        #region Utilities

        private void PreventMultipleInitialize()
        {
            if (_isInitializeCalledBefore)
            {
                throw new InvalidOperationException("This instance has started before. Can not call Initialize method more than once.");
            }

            _isInitializeCalledBefore = true;
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