using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Application.Models.Catalog;

namespace Confetti.Application.Catalog
{
    /// <summary>
    /// Product service
    /// </summary>
    public partial interface IProductService
    {
        /// <summary>
        /// Search products
        /// </summary>
        /// <param name="loadFilterableCountableSpecificationAttributeOptionIds">a value indicating whether we should load the specification attribute option identifiers applied to loaded products (all pages) with count of products</param>
        /// <param name="loadMinMaxPrices">a value indicating whether we should load min and max prices (all pages)</param>
        /// <param name="searchModel">search product model</param>
        /// <returns>Products</returns>
        Task<ProductsSearchModelResult> SearchProductsAsync(
            bool loadFilterableCountableSpecificationAttributeOptionIds = false,
            bool loadMinMaxPrices = false,
            ProductsSearchModel searchModel = null
        );
    }
}