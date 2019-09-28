using Confetti.Domain.Configuration;
using Confetti.Domain.Entities;
using Confetti.Domain.Linq;
using Confetti.Domain.Repositories;
using Confetti.Domain.Uow;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a extensions for <see cref="ConfettiDomainBuilder"/>.
    /// </summary>
    public static class ConfettiDomainBuilderExtensionsAdditional
    {
        /// <summary>
        /// Adds a unit of work.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of the unit of work.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddUnitOfWork<T>(this ConfettiDomainBuilder builder)
            where T : class, IUnitOfWork
        {
            builder.Services.AddTransient<IUnitOfWork, T>();

            return builder;
        }

        /// <summary>
        /// Adds a unit of work manager.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of the unit of work manager.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddUnitOfWorkManager<T>(this ConfettiDomainBuilder builder)
            where T : class, IUnitOfWorkManager
        {
            builder.Services.AddScoped<IUnitOfWorkManager, T>();

            return builder;
        }

        /// <summary>
        /// Adds a connection string resolver.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of the connection string resolver.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddConnectionStringResolver<T>(this ConfettiDomainBuilder builder)
            where T : class, IConnectionStringResolver
        {
            builder.Services.AddTransient<IConnectionStringResolver, T>();

            return builder;
        }

        /// <summary>
        /// Adds a entity repository.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of the entity repository.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddEntityRepository<T, TEntity, TPrimaryKey>(this ConfettiDomainBuilder builder)
            where T : class, IRepository<TEntity, TPrimaryKey>
            where TEntity : class, IEntity<TPrimaryKey>
        {
            builder.Services.AddTransient<IRepository<TEntity, TPrimaryKey>, T>();

            return builder;
        }

        /// <summary>
        /// Adds a async queryable executer.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="T">The type of the async queryable executer.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddAsyncQueryableExecuter<T>(this ConfettiDomainBuilder builder)
            where T : class, IAsyncQueryableExecuter
        {
            builder.Services.AddTransient<IAsyncQueryableExecuter, T>();

            return builder;
        }
    }
}