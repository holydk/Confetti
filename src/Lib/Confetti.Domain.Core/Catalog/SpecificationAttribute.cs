using System.Collections.Generic;

namespace Confetti.Domain.Core.Catalog
{
    /// <summary>
    /// Represents a specification attribute
    /// </summary>
    public partial class SpecificationAttribute
    {
        private ICollection<SpecificationAttributeOption> _specificationAttributeOptions;

        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the position
        /// </summary>
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets the specification attribute options
        /// </summary>
        public virtual ICollection<SpecificationAttributeOption> SpecificationAttributeOptions
        {
            get => _specificationAttributeOptions ?? (_specificationAttributeOptions = new List<SpecificationAttributeOption>());
            protected set => _specificationAttributeOptions = value;
        }
    }
}