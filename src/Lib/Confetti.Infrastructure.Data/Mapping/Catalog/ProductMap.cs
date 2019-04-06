using Confetti.Domain.Core.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product mapping configuration
    /// </summary>
    public partial class ProductMap : ConfettiEntityTypeConfiguration<Product>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable(nameof(Product));
            builder.HasKey(product => product.Id);

            builder.Property(product => product.Name).HasMaxLength(400).IsRequired();
            builder.Property(product => product.MetaKeywords).HasMaxLength(400);
            builder.Property(product => product.MetaTitle).HasMaxLength(400);
            builder.Property(product => product.Sku).HasMaxLength(400);
            builder.Property(product => product.ManufacturerPartNumber).HasMaxLength(400);
            builder.Property(product => product.AdditionalShippingCharge).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.Price).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.OldPrice).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.ProductCost).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.Weight).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.Length).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.Width).HasColumnType("decimal(18, 4)");
            builder.Property(product => product.Height).HasColumnType("decimal(18, 4)");

            // Indexes
            // Get low stock products
            builder.HasIndex(c => new 
            { 
                c.Deleted, 
                c.ManageInventoryMethodId, 
                c.MinStockQuantity, 
                c.UseMultipleWarehouses 
            });
            builder.HasIndex(c => new 
            { 
                c.Deleted, 
                c.Id 
            });
            builder.HasIndex(c => new 
            { 
                c.Published, 
                c.Deleted 
            });
            builder.HasIndex(c => new 
            { 
                c.Price, 
                c.AvailableStartDateTimeUtc,
                c.AvailableEndDateTimeUtc,
                c.Published,
                c.Deleted 
            });
            builder.HasIndex(c => c.Published);
            builder.HasIndex(c => c.ShowOnHomePage);

            base.Configure(builder);
        }

        #endregion
    }
}