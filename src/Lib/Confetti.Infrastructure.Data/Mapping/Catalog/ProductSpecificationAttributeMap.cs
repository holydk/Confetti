using Confetti.Domain.Catalog;
using Confetti.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Nop.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product specification attribute mapping configuration
    /// </summary>
    public partial class ProductSpecificationAttributeMap : ConfettiEntityTypeConfiguration<ProductSpecificationAttribute>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductSpecificationAttribute> builder)
        {
            builder.ToTable(ConfettiMappingDefaults.ProductSpecificationAttributeTable);
            builder.HasKey(productSpecificationAttribute => new
            {
                productSpecificationAttribute.ProductId,
                productSpecificationAttribute.AttributeTypeId,
                productSpecificationAttribute.SpecificationAttributeOptionId
            });

            builder.Property(productSpecificationAttribute => productSpecificationAttribute.CustomValue).HasMaxLength(1000);

            // Relations
            builder.HasOne(productSpecificationAttribute => productSpecificationAttribute.SpecificationAttributeOption)
                .WithMany()
                .HasForeignKey(productSpecificationAttribute => productSpecificationAttribute.SpecificationAttributeOptionId)
                .IsRequired();

            builder.HasOne(productSpecificationAttribute => productSpecificationAttribute.Product)
                .WithMany(product => product.ProductSpecificationAttributes)
                .HasForeignKey(productSpecificationAttribute => productSpecificationAttribute.ProductId)
                .IsRequired();

            // Indexes
            builder.HasIndex(productSpecificationAttribute => productSpecificationAttribute.ProductId);
            builder.HasIndex(productSpecificationAttribute => productSpecificationAttribute.SpecificationAttributeOptionId);
            builder.HasIndex(productSpecificationAttribute => productSpecificationAttribute.AllowFiltering);
            builder.HasIndex(productSpecificationAttribute => new
            {
                productSpecificationAttribute.SpecificationAttributeOptionId,
                productSpecificationAttribute.AllowFiltering
            });

            base.Configure(builder);
        }

        #endregion
    }
}