using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Confetti.Catalog.Application;
using Confetti.Catalog.Application.Models;
using Confetti.Catalog.Domain.Models;
using Confetti.Collections;
using Confetti.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Application
{
    /// <summary>
    /// Represents default implementation of category service.
    /// </summary>
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;

        #endregion

        #region Ctor

        public CategoryService(
            IMapper mapper,
            IRepository<Category> categoryRepository
        )
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
            
        #endregion

        #region Methods

        public virtual Task<List<Category>> GetAllListAsync(Expression<Func<Category, bool>> predicate)
        {
            return Task.FromResult(_categoryRepository.Table.ToList());
        }

        public virtual T Query<T>(Func<IQueryable<Category>, T> queryMethod)
        {
            return queryMethod(_categoryRepository.Table);
        }

        /// <summary>
        /// Gets all categories by page options.
        /// </summary>
        /// <param name="pageIndex"> The page index. </param>
        /// <param name="pageSize"> The page size. </param>
        /// <returns> The paged list of categories. </returns>
        public async Task<IPagedList<CategoryModel>> GetCategoriesAsync(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _categoryRepository.Table;

            query = query.OrderBy(c => c.ParentId)
                .ThenBy(c => c.Position)
                .ThenBy(c => c.Id);

            // map
            var mappedModels = query.ProjectTo<CategoryModel>(_mapper.ConfigurationProvider);

            

            // run query
            var unsortedCategories = await mappedModels.ToArrayAsync();

            return new PagedList<CategoryModel>(
                unsortedCategories,
                pageIndex,
                pageSize);
        }
            
        #endregion
    }
}
