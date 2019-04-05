using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Models.Catalog;
using Confetti.Application.Models.Catalog;

namespace Confetti.Api.Areas.Public.Factories
{
    public partial interface ICatalogModelFactory
    {
        /// <summary>
        /// Prepare top menu model
        /// </summary>
        /// <returns>Top menu model</returns>
        Task<TopMenuModel> PrepareTopMenuModelAsync();

        /// <summary>
        /// Prepare category (simple) models
        /// </summary>
        /// <param name="rootCategoryId">Root category identifier</param>
        /// <param name="loadSubCategories">A value indicating whether subcategories should be loaded</param>
        /// <returns>List of category (simple) models</returns>
        Task<List<CategorySimpleModel>> PrepareCategorySimpleModelsAsync(int? rootCategoryId, bool loadSubCategories = true);

        /// <summary>
        /// Prepare top menu categories
        /// </summary>
        /// <returns>List of category (simple) models</returns>
        Task<List<CategorySimpleModel>> PrepareTopMenuCategoriesAsync();

        /// <summary>
        /// Prepare category home model
        /// </summary>
        /// <param name="model">Root category model</param>
        /// <returns>home model</returns>
        Task<CategoryHomeModel> PrepareCategoryHomeModelAsync(CategoryModel model);

        /// <summary>
        /// Prepare public category
        /// </summary>
        /// <param name="model">category model</param>
        /// <returns>public category</returns>
        Task<CategoryPublicModel> PrepareCategoryPublicModelAsync(CategoryModel model);
    }
}