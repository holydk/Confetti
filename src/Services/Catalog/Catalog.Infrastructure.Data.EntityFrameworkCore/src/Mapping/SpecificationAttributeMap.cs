using Confetti.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a specification attribute mapping configuration
    /// </summary>
    public partial class SpecificationAttributeMap : CatalogEntityTypeConfiguration<SpecificationAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<SpecificationAttribute> builder)
        {
            builder.ToTable(nameof(SpecificationAttribute));
            builder.HasKey(attribute => attribute.Id);

            builder.Property(attribute => attribute.Name).HasMaxLength(255).IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}