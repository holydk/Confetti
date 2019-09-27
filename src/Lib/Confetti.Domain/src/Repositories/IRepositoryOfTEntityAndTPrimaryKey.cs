using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Confetti.Domain.Entities;

namespace Confetti.Domain.Repositories
{
    /// <summary>
    /// Provides an abstraction of the entity repository.
    /// </summary>
    /// <typeparam name="TEntity">Main Entity type this repository works on.</typeparam>
    /// <typeparam name="TPrimaryKey">Primary key type of the entity.</typeparam>
    public interface IRepository<TEntity, TPrimaryKey>  where TEntity : class, IEntity<TPrimaryKey>
    {
        #region Select/Get/Query

        /// <summary>
        /// Gets a IQueryable that is used to retrieve entities from entire table.
        /// </summary>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        IQueryable<TEntity> GetAll();

        /// <summary>
        /// Gets a IQueryable that is used to retrieve entities from entire table.
        /// One or more 
        /// </summary>
        /// <param name="propertySelectors">A list of include expressions.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors);

        /// <summary>
        /// Creates a LINQ query for the entity based on a raw SQL query.
        /// </summary>
        /// <param name="sql">The raw SQL query.</param>
        /// <param name="parameters">The values to be assigned to parameters.</param>
        /// <returns>IQueryable to be used to select entities from database.</returns>
        Task<IQueryable<TEntity>> FromSqlAsync(string sql, params DbParameter[] parameters);

        /// <summary>
        /// Gets an entity with given primary key.
        /// </summary>
        /// <param name="id">Primary key of the entity to get.</param>
        /// <returns>The entity.</returns>
        Task<TEntity> GetByIdAsync(TPrimaryKey id);

        #endregion

        #region Insert

        /// <summary>
        /// Inserts a new entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Inserts a new entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        Task InsertAsync(IEnumerable<TEntity> entities);
            
        #endregion

        #region Update

        /// <summary>
        /// Updates a existing entity. 
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task UpdateAsync(TEntity entity);

        /// <summary>
        /// Updates a existing entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        Task UpdateAsync(IEnumerable<TEntity> entities);
            
        #endregion

        #region Delete

        /// <summary>
        /// Deletes a existing entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        Task DeleteAsync(TEntity entity);

        /// <summary>
        /// Deletes a existing entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        Task DeleteAsync(IEnumerable<TEntity> entities);
            
        #endregion
    }
}