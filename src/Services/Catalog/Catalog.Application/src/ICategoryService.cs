using System.Threading.Tasks;
using Confetti.Catalog.Application.Models;
using Confetti.Collections;

namespace Confetti.Catalog.Application
{
    /// <summary>
    /// Represents a category service.
    /// </summary>
    public interface ICategoryService
    {
        /// <summary>
        /// Gets all categories by page options.
        /// </summary>
        /// <param name="pageIndex"> The page index. </param>
        /// <param name="pageSize"> The page size. </param>
        /// <returns> The paged list of categories. </returns>
        Task<IPagedList<CategoryModel>> GetCategoriesAsync(int pageIndex = 0, int pageSize = int.MaxValue);
    }
}
