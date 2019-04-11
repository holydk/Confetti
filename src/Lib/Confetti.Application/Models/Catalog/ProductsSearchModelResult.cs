using System.Collections.Generic;
using Confetti.Application.Collections;

namespace Confetti.Application.Models.Catalog
{
    /// <summary>
    /// Represents a product search model result
    /// </summary>
    public partial class ProductsSearchModelResult
    {
        /// <summary>
        /// Gets or sets products found
        /// </summary>
        public IPagedList<ProductModel> Products { get; set; }

        /// <summary>
        /// Gets or sets a specification attribute option ids with count of products
        /// </summary>
        public IList<CountableSpecificationAttributeOption> CountableSpecificationAttributeOptionIds { get; set; }

        /// <summary>
        /// Gets or sets a min price by products found
        /// </summary>
        public decimal MinPrice { get; set; }

        /// <summary>
        /// Gets or sets a max price by products found
        /// </summary>
        public decimal MaxPrice { get; set; }
    }
}