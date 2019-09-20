namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents default values related to data mapping
    /// </summary>
    public static partial class CatalogMappingDefaults
    {
        /// <summary>
        /// Gets a name of the Product-Category mapping table
        /// </summary>
        public static string ProductCategoryTable => "Product_Category_Mapping";

        /// <summary>
        /// Gets a name of the Product-SpecificationAttribute mapping table
        /// </summary>
        public static string ProductSpecificationAttributeTable => "Product_SpecificationAttribute_Mapping";
    }
}