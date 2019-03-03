using System.Collections.Generic;
using Confetti.Application.Collections;
using Confetti.Application.Models.Catalog;

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
        IList<CategoryModel> GetAllCategories(int storeId = 0, bool showHidden = false, bool loadCacheableCopy = true);

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="title">Category title</param>
        /// <param name="storeId">Store identifier; 0 if you want to get all records</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Categories</returns>
        IPagedList<CategoryModel> GetAllCategories(string title, int storeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, bool showHidden = false);
    }
}