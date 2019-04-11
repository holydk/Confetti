using System.Collections.Generic;

namespace Confetti.Application.Models.Catalog
{
    /// <summary>
    /// Represents a product search model
    /// </summary>
    public partial class ProductsSearchModel
    {
        /// <summary>
        /// Gets or sets product keywords.
        /// Default value is null.
        /// </summary>
        public string Keywords { get; set; } = null;

        /// <summary>
        /// Gets or sets page index.
        /// Default value is 0.
        /// </summary>
        public int PageIndex { get; set; } = 0;

        /// <summary>
        /// Gets or sets page size.
        /// Default value is 2147483647.
        /// </summary>
        public int PageSize { get; set; } = int.MaxValue;

        /// <summary>
        /// Gets or sets price min.
        /// Default value is null.
        /// </summary>
        public decimal? PriceMin { get; set; } = null;

        /// <summary>
        /// Gets or sets price max.
        /// Default value is null.
        /// </summary>
        public decimal? PriceMax { get; set; } = null;

        /// <summary>
        /// Gets or sets Category identifiers.
        /// Default value is null.
        /// </summary>
        public IList<int> CategoryIds { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether to load only products marked as "new"; 
        /// "false" to load all records; "true" to load "marked as new" only.
        /// Default value is false.
        /// </summary>
        public bool MarkedAsNewOnly { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to search by a specified "keyword" in product SKU
        /// Default value is true.
        /// </summary>
        public bool SearchSku { get; set; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to search by a specified "keyword" in product descriptions
        /// Default value is false.
        /// </summary>
        public bool SearchDescriptions { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether loaded products are marked as featured (relates only to categories). 
        /// 0 to load featured products only, 1 to load not featured products only, null to load all products
        /// Default value is null.
        /// </summary>
        public bool? FeaturedProducts { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether to show hidden records.
        /// Default value is false.
        /// </summary>
        public bool ShowHidden { get; set; } = false;

        /// <summary>
        /// Gets or sets a value indicating whether to need override published field.
        /// null - process "Published" property according to "showHidden" parameter
        /// true - load only "Published" products
        /// false - load only "Unpublished" products
        /// Default value is null.
        /// </summary>
        public bool? OverridePublished { get; set; } = null;

        /// <summary>
        /// Filtered product specification identifiers.
        /// Default value is null.
        /// </summary>
        public IList<int> FilteredSpecs { get; set; } = null;
        
        /// <summary>
        /// Gets or sets a warehouse identifier; 0 to load all records.
        /// Default value is 0.
        /// </summary>
        public int WarehouseId { get; set; } = 0;

        /// <summary>
        /// Order by.
        /// Default value is ProductSortingEnum.Position.
        /// </summary>
        public ProductSortingEnum OrderBy { get; set; } = ProductSortingEnum.Position;
    }
}