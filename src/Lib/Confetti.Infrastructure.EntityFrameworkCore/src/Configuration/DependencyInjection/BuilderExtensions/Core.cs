using Confetti.Domain.Configuration;
using Confetti.Domain.Entities;
using Confetti.Domain.Repositories;
using Confetti.Infrastructure.EntityFrameworkCore;
using Confetti.Infrastructure.EntityFrameworkCore.Linq;
using Confetti.Infrastructure.EntityFrameworkCore.Repositories;
using Confetti.Infrastructure.EntityFrameworkCore.Uow;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a extensions for <see cref="ConfettiDomainBuilder"/>.
    /// </summary>
    public static class EfCoreConfettiDomainBuilderExtensionsCore
    {
        /// <summary>
        /// Adds a ef core domain services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddEntityFrameworkCore(this ConfettiDomainBuilder builder)
        {
            builder
                .AddUnitOfWork<EfCoreUnitOfWork>()
                .AddAsyncQueryableExecuter<EfCoreAsyncQueryableExecuter>();

            builder.Services.TryAddTransient<IEfCoreTransactionStrategy, DbContextEfCoreTransactionStrategy>();
            builder.Services.TryAddScoped<IActiveDbContextsService, ActiveDbContextsService>();

            return builder;
        }

        /// <summary>
        /// Adds a db context provider.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <typeparam name="TContext">The type of the db context.</typeparam>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddDbContextProvider<TContext>(this ConfettiDomainBuilder builder)
            where TContext : DbContext
        {
            builder.Services.AddTransient<IDbContextProvider<TContext>, DbContextProvider<TContext>>();

            return builder;
        }

       /// <summary>
       /// Adds a ef core entity repository.
       /// </summary>
       /// <param name="builder">The builder.</param>
       /// <typeparam name="TContext">The type of the db context.</typeparam>
       /// <typeparam name="TEntity">The type of the entity.</typeparam>
       /// <typeparam name="TPrimaryKey">The type of the primary key.</typeparam>
       /// <returns></returns>
        public static ConfettiDomainBuilder AddEfCoreEntityRepository<TContext, TEntity, TPrimaryKey>(this ConfettiDomainBuilder builder)
            where TEntity : class, IEntity<TPrimaryKey>
            where TContext : DbContext
        {
            builder.Services.AddTransient<IRepository<TEntity, TPrimaryKey>, EfCoreRepository<TContext, TEntity, TPrimaryKey>>();

            return builder;
        }
    }
}