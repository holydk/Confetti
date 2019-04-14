using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Factories;
using Confetti.Application.Catalog;
using Confetti.Framework.Controllers;
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
            return Ok(categories);
        }

        [HttpGet]
        [Route("Category/Home/{rootCategoryId}")]
        public async Task<IActionResult> GetCategoryHomeAsync(int rootCategoryId)
        {
            // todo: add validation
            var category = await _categoryService.GetCategoryByIdAsync(rootCategoryId);

            if (category == null)
                return NotFound();

            var categoryhomeModel = await _catalogModelFactory.PrepareCategoryHomeModelAsync(category);

            return Ok(categoryhomeModel);
        }

        [HttpGet]
        [Route("Category/{categoryId}")]
        public async Task<IActionResult> GetCategoryAsync(int categoryId)
        {
            // todo: add validation
            var category = await _categoryService.GetCategoryByIdAsync(categoryId);

            if (category == null)
                return NotFound(null);

            var categoryModel = await _catalogModelFactory.PrepareCategoryPublicModelAsync(category);

            return Ok(categoryModel);
        }
            
        #endregion
    }
}