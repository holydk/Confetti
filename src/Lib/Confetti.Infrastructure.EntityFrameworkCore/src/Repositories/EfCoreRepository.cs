using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Confetti.Domain.Entities;
using Confetti.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.EntityFrameworkCore.Repositories
{
    /// <summary>
    /// Provides an implementation of the EntityFrameworkCore entity repository.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on.</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity.</typeparam>
    public class EfCoreRepository<TContext, TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey> 
        where TEntity : class, IEntity<TPrimaryKey>
        where TContext : DbContext
    {
        #region Fields

        private readonly IDbContextProvider<TContext> _dbContextProvider;
        private DbSet<TEntity> _entities;
            
        #endregion

        #region Properties

        protected virtual TContext Context => _dbContextProvider.Get();

        /// <summary>
        /// Gets an entity set
        /// </summary>
        protected virtual DbSet<TEntity> Table
        {
            get
            {
                if (_entities == null)
                    _entities = Context.Set<TEntity>();

                return _entities;
            }
        }
            
        #endregion

        #region Ctor

        public EfCoreRepository(IDbContextProvider<TContext> dbContextProvider)
        {
            _dbContextProvider = dbContextProvider;
        }
            
        #endregion

        #region Select/Get/Query

        /// <summary>
        /// Gets a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public IQueryable<TEntity> GetAll()
        {
            return Table;
        }

        /// <summary>
        /// Gets a IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = Table.AsQueryable();

            if (propertySelectors != null && propertySelectors.Length > 0)
            {
                foreach (var propertySelector in propertySelectors)
                {
                    query = query.Include(propertySelector);
                }
            }

            return query;
        }

        /// <summary>
        /// Creates a LINQ query for the entity based on a raw SQL query.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="parameters">The values to be assigned to parameters.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        public Task<IQueryable<TEntity>> FromSqlAsync(string sql, params DbParameter[] parameters)
        {
            if (string.IsNullOrWhiteSpace(sql)) throw new ArgumentNullException(nameof(sql));
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));

            return Task.FromResult(Context.EntityFromSql<TEntity>(sql, parameters));
        }

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get.</param>
        /// <returns>The entity.</returns>
        public Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return Table.FirstOrDefaultAsync(CreateEqualityExpressionForId(id));
        }
            
        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public Task InsertAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            return Table.AddAsync(entity).AsTask();
        }

        /// <summary>
        /// Inserts a new entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public Task InsertAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            return Table.AddRangeAsync(entities);
        }
            
        #endregion

        #region Update

        /// <summary>
        /// Updates a existing entity. 
        /// </summary>
        /// <param name="entity">The entity.</param>
        public Task UpdateAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Table.Update(entity);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Updates a existing entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public Task UpdateAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));
            
            Table.UpdateRange(entities);

            return Task.CompletedTask;
        }
            
        #endregion

        #region Delete

        /// <summary>
        /// Deletes a existing entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public Task DeleteAsync(TEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            Table.Remove(entity);

            return Task.CompletedTask;
        }

        /// <summary>
        /// Deletes a existing entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public Task DeleteAsync(IEnumerable<TEntity> entities)
        {
            if (entities == null) throw new ArgumentNullException(nameof(entities));

            Table.RemoveRange(entities);

            return Task.CompletedTask;
        }
            
        #endregion

        #region Utilities

        protected virtual Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        {
            var lambdaParam = Expression.Parameter(typeof(TEntity));

            var leftExpression = Expression.PropertyOrField(lambdaParam, "Id");

            Expression<Func<object>> closure = () => id;
            var rightExpression = Expression.Convert(closure.Body, leftExpression.Type);

            var lambdaBody = Expression.Equal(leftExpression, rightExpression);

            return Expression.Lambda<Func<TEntity, bool>>(lambdaBody, lambdaParam);
        }
            
        #endregion
    }
}