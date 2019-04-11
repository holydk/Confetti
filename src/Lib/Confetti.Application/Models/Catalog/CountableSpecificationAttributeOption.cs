namespace Confetti.Application.Models.Catalog
{
    /// <summary>
    /// Represents a specification attribute option id with count of products
    /// </summary>
    public class CountableSpecificationAttributeOption
    {
        /// <summary>
        /// Gets or sets a specification attribute option id
        /// </summary>
        public int SpecificationAttributeOptionId { get; set; }

        /// <summary>
        /// Gets or sets a products count
        /// </summary>
        public int CountOfProducts { get; set; }
    }
}