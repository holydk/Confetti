using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Application.Models.Catalog;
using Confetti.Domain.Core.Collections;

namespace Confetti.Application.Catalog
{
    /// <summary>
    /// Category service interface
    /// </summary>
    public partial interface ICategoryService
    {
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="loadCacheableCopy">A value indicating whether to load a copy that could be cached (workaround until Entity Framework supports 2-level caching)</param>
        /// <returns>Categories</returns>
        Task<IList<CategoryModel>> GetAllCategoriesAsync(int storeId = 0, bool showHidden = false, bool loadCacheableCopy = true);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="title">Category title</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        Task<IPagedList<CategoryModel>> GetAllCategoriesAsync(string title, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        Task<CategoryModel> GetCategoryByIdAsync(int categoryId);

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="rootCategoryId">Category identifier</param>
        /// <returns>Categories</returns>
        Task<IList<CategoryModel>> GetAllCategoriesDisplayedOnHomePageAsync(int rootCategoryId);

        /// <summary>
        /// Get category breadcrumb by category
        /// </summary>
        /// <param name="model">category</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>category breadcrumb</returns>
        Task<IList<CategoryModel>> GetCategoryBreadCrumbAsync(CategoryModel model, bool showHidden = false);
    }
}