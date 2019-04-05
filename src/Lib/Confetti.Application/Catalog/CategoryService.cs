using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Confetti.Application;
using Confetti.Application.Caching;
using Confetti.Application.Collections;
using Confetti.Application.Models.Catalog;
using Confetti.Domain.Core.Catalog;
using Confetti.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Application.Catalog
{
    /// <summary>
    /// Category service interface
    /// </summary>
    public class CategoryService : ICategoryService
    {
        #region Fields

        private readonly IMapper _mapper;
        private readonly IRepository<Category> _categoryRepository;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public CategoryService(
            IMapper mapper,
            IRepository<Category> categoryRepository,
            IStaticCacheManager staticCacheManager
        )
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _staticCacheManager = staticCacheManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Categories</returns>
        public async Task<IList<CategoryModel>> GetAllCategoriesAsync(
            int storeId = 0, 
            bool showHidden = false, 
            bool loadCacheableCopy = true)
        {
            Task<IPagedList<CategoryModel>> loadCategoriesFuncAsync() => 
                GetAllCategoriesAsync(string.Empty, storeId, showHidden: showHidden);

            IList<CategoryModel> categories;
            if (loadCacheableCopy)
            {
                //cacheable copy
                var key = string.Format(ConfettiCatalogDefaults.CategoriesAllCacheKey,
                    storeId,
                    //string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
                    "customRole",
                    showHidden);
                categories = await _staticCacheManager.GetAsync(key, loadCategoriesFuncAsync);
            }
            else
            {
                categories = await loadCategoriesFuncAsync();
            }

            return categories;
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="title">Category title</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        public async Task<IPagedList<CategoryModel>> GetAllCategoriesAsync(
            string title, 
            int storeId = 0,
            int pageIndex = 0, 
            int pageSize = int.MaxValue, 
            bool showHidden = false)
        {
            // LINQ
            var query = _categoryRepository.Table;
            if (!showHidden)
                query = query.Where(c => c.Published);
            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(c => c.Title.Contains(title));
            query = query.Where(c => !c.Deleted);
            query = query.OrderBy(c => c.ParentId)
                .ThenBy(c => c.Position)
                .ThenBy(c => c.Id);

            // map
            var mappedModels = query.ProjectTo<CategoryModel>(_mapper.ConfigurationProvider);

            // run query
            var unsortedCategories = await mappedModels.ToListAsync();

            return new PagedList<CategoryModel>(
                unsortedCategories,
                pageIndex,
                pageSize);
        }

        #endregion
    }
}