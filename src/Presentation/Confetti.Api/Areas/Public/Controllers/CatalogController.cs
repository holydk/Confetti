using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Factories;
using Confetti.Api.Controllers;
using Confetti.Application.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Confetti.Api.Areas.Public.Controllers
{
    public class CatalogController : ApiController
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly ICatalogModelFactory _catalogModelFactory;

        #endregion

        #region Ctor

        public CatalogController(
            ICategoryService categoryService,
            ICatalogModelFactory catalogModelFactory)
        {
            _categoryService = categoryService;
            _catalogModelFactory = catalogModelFactory;         
        }
            
        #endregion

        #region Categories

        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();    
            return Response(categories);
        }

        [HttpGet]
        [Route("Category/Home/{rootCategoryId}")]
        public async Task<IActionResult> GetCategoryHomeAsync(int rootCategoryId)
        {
            // todo: add validation
            var category = await _categoryService.GetCategoryByIdAsync(rootCategoryId);

            if (category == null)
                return Response(null);

            var categoryhomeModel = await _catalogModelFactory.PrepareCategoryHomeModelAsync(category);

            return Response(categoryhomeModel);
        }

        [HttpGet]
        [Route("Category/{categoryId}")]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            // todo: add validation
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return Response(null);

            var categoryModel = await _catalogModelFactory.PrepareCategoryPublicModelAsync(category);

            return Response(categoryModel);
        }
            
        #endregion
    }
}