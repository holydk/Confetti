using Confetti.Domain.Core.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product category mapping configuration
    /// </summary>
    public partial class ProductCategoryMap: ConfettiEntityTypeConfiguration<ProductCategory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.ToTable(ConfettiMappingDefaults.ProductCategoryTable);
            builder.HasKey(productCategory => new 
            { 
                productCategory.CategoryId, 
                productCategory.ProductId 
            });
            
            // Relations
            builder.HasOne(productCategory => productCategory.Category)
                .WithMany()
                .HasForeignKey(productCategory => productCategory.CategoryId)
                .IsRequired();

            builder.HasOne(productCategory => productCategory.Product)
                .WithMany(product => product.ProductCategories)
                .HasForeignKey(productCategory => productCategory.ProductId)
                .IsRequired();

            // Indexes
            builder.HasIndex(productCategory => productCategory.CategoryId);
            builder.HasIndex(productCategory => productCategory.ProductId);
            builder.HasIndex(productCategory => new 
            {
                productCategory.ProductId,
                productCategory.IsFeaturedProduct
            });
            builder.HasIndex(productCategory => productCategory.IsFeaturedProduct);

            base.Configure(builder);
        }

        #endregion
    }
}