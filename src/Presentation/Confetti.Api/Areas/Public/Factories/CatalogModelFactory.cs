using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Models.Catalog;
using Confetti.Api.Infrastructure.Cache;
using Confetti.Application.Catalog;
using Confetti.Application.Models.Catalog;
using Confetti.Infrastructure.Caching;

namespace Confetti.Api.Areas.Public.Factories
{
    public class CatalogModelFactory : ICatalogModelFactory
    {
        #region Fields

        private readonly ICategoryService _categoryService;
        private readonly IStaticCacheManager _cacheManager;

        #endregion

        #region Ctor

        public CatalogModelFactory(
            ICategoryService categoryService,
            IStaticCacheManager cacheManager
        )
        {
            _categoryService = categoryService;
            _cacheManager = cacheManager;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Prepare top menu model
        /// </summary>
        /// <returns>Top menu model</returns>
        public async Task<TopMenuModel> PrepareTopMenuModelAsync()
        {
            var cachedCategoriesModel = await PrepareTopMenuCategoriesAsync();
            var model = new TopMenuModel
            {
                Categories = cachedCategoriesModel
            };

            return model;
        }

        /// <summary>
        /// Prepare category (simple) models
        /// </summary>
        /// <param name="rootCategoryId">Root category identifier</param>
        /// <param name="loadSubCategories">A value indicating whether subcategories should be loaded</param>
        /// <returns>List of category (simple) models</returns>
        public async Task<List<CategorySimpleModel>> PrepareCategorySimpleModelsAsync(int? rootCategoryId, bool loadSubCategories = true)
        {
            var result = new List<CategorySimpleModel>();

            //little hack for performance optimization
            //we know that this method is used to load top and left menu for categories.
            //it'll load all categories anyway.
            //so there's no need to invoke "GetAllCategoriesByParentCategoryId" multiple times (extra SQL commands) to load childs
            //so we load all categories at once (we know they are cached)
            
            // todo: add StoreContext
            // var allCategories = _categoryService.GetAllCategories(storeId: _storeContext.CurrentStore.Id);

            var allCategories = await _categoryService.GetAllCategoriesAsync(storeId: 0);
            var categories = allCategories.Where(c => c.ParentId == rootCategoryId).ToList();
            foreach (var category in categories)
            {
                var categoryModel = new CategorySimpleModel
                {
                    Title = category.Title
                };

                // todo: add number of products

                if (loadSubCategories)
                {
                    var subCategories = await PrepareCategorySimpleModelsAsync(category.Id, loadSubCategories);
                    categoryModel.SubCategories.AddRange(subCategories);
                }

                categoryModel.HaveSubCategories = categoryModel.SubCategories.Count > 0;

                result.Add(categoryModel);
            }

            return result;
        }

        /// <summary>
        /// Prepare top menu categories
        /// </summary>
        /// <returns>List of category (simple) models</returns>
        public Task<List<CategorySimpleModel>> PrepareTopMenuCategoriesAsync()
        {
            //load and cache them
            var cacheKey = string.Format(ConfettiModelCacheDefaults.CategoryAllModelKey,
                //string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
                "customRole",
                //_storeContext.CurrentStore.Id
                0);
            return _cacheManager.GetAsync(cacheKey, () => PrepareIndludedInTopMenuCategoriesAsync(null));
        }

        /// <summary>
        /// Prepare category home model
        /// </summary>
        /// <param name="model">Root category model</param>
        /// <returns>home model</returns>
        public async Task<CategoryHomeModel> PrepareCategoryHomeModelAsync(CategoryModel model)
        {
            var homeModel = new CategoryHomeModel()
            {
                Title = model.Title,
                Description = model.Description,
                MetaTitle = model.MetaTitle,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
                Categories = await PrepareHomeCategoriesAsync(model.Id)
            };

            return homeModel;
        }

        /// <summary>
        /// Prepare public category
        /// </summary>
        /// <param name="model">category model</param>
        /// <returns>public category</returns>
        public async Task<CategoryPublicModel> PrepareCategoryPublicModelAsync(CategoryModel model)
        {
            var categoryModel = new CategoryPublicModel()
            {
                Title = model.Title,
                Description = model.Description,
                MetaTitle = model.MetaTitle,
                MetaDescription = model.MetaDescription,
                MetaKeywords = model.MetaKeywords,
            };
            
            // set category breadcrumb
            var breadcrumbCacheKey = string.Format(ConfettiModelCacheDefaults.CategoryBreadcrumbKey,
                model.Id,
                //string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
                "customRole",
                "0");
            categoryModel.CategoryBreadcrumb = await _cacheManager.GetAsync(
                breadcrumbCacheKey, 
                async () =>
                {
                    var query = from b in await _categoryService.GetCategoryBreadCrumbAsync(model)
                                select new CategorySimpleModel()
                                {
                                    Title = b.Title,
                                    Route = b.Id.ToString()
                                };

                    return query.ToList();
                }
            );

            return categoryModel;
        }

        #endregion

        #region Utilities

        private Task<List<CategoryPublicModel>> PrepareHomeCategoriesAsync(int rootCategoryId)
        {
            //load and cache them
            var cacheKey = string.Format(ConfettiModelCacheDefaults.CategoryHomepageKey,
                //string.Join(",", _workContext.CurrentCustomer.GetCustomerRoleIds()),
                "customRole",
                //_storeContext.CurrentStore.Id
                "0",
                rootCategoryId.ToString());
            return _cacheManager.GetAsync(
                cacheKey, 
                async () => 
                {
                    var query = from c in await _categoryService.GetAllCategoriesDisplayedOnHomePageAsync(rootCategoryId)
                                select new CategoryPublicModel()
                                {
                                    Title = c.Title,
                                    Description = c.Description,
                                    MetaTitle = c.MetaTitle,
                                    MetaDescription = c.MetaDescription,
                                    MetaKeywords = c.MetaKeywords,
                                };

                    return query.ToList();
                });
        }

        private async Task<List<CategorySimpleModel>> PrepareIndludedInTopMenuCategoriesAsync(int? rootCategoryId, bool loadSubCategories = true)
        {
            var result = new List<CategorySimpleModel>();

            //little hack for performance optimization
            //we know that this method is used to load top and left menu for categories.
            //it'll load all categories anyway.
            //so there's no need to invoke "GetAllCategoriesByParentCategoryId" multiple times (extra SQL commands) to load childs
            //so we load all categories at once (we know they are cached)
            
            // todo: add StoreContext
            // var allCategories = _categoryService.GetAllCategories(storeId: _storeContext.CurrentStore.Id);

            var allCategories = await _categoryService.GetAllCategoriesAsync(storeId: 0);
            var categories = allCategories.Where(
                c => c.ParentId == rootCategoryId && c.IsIncludeInTopMenu).ToList();
            foreach (var category in categories)
            {
                var categoryModel = new CategorySimpleModel
                {
                    Title = category.Title,
                    Route = category.Id.ToString()
                };

                // todo: add number of products

                if (loadSubCategories)
                {
                    var subCategories = await PrepareIndludedInTopMenuCategoriesAsync(category.Id, loadSubCategories);
                    categoryModel.SubCategories.AddRange(subCategories);
                }

                categoryModel.HaveSubCategories = categoryModel.SubCategories.Count > 0;

                result.Add(categoryModel);
            }

            return result;
        }
            
        #endregion
    }
}