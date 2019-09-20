using Confetti.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a product warehouse inventory mapping configuration
    /// </summary>
    public partial class ProductWarehouseInventoryMap : CatalogEntityTypeConfiguration<ProductWarehouseInventory>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<ProductWarehouseInventory> builder)
        {
            builder.ToTable(nameof(ProductWarehouseInventory));
            builder.HasKey(productWarehouseInventory => new 
            {
                productWarehouseInventory.ProductId,
                productWarehouseInventory.WarehouseId
            });

            // Relations
            builder.HasOne(productWarehouseInventory => productWarehouseInventory.Product)
                .WithMany(product => product.ProductWarehouseInventory)
                .HasForeignKey(productWarehouseInventory => productWarehouseInventory.ProductId)
                .IsRequired();

            // Indexes
            builder.HasIndex(productWarehouseInventory => productWarehouseInventory.ProductId);
            builder.HasIndex(productWarehouseInventory => productWarehouseInventory.WarehouseId);

            base.Configure(builder);
        }

        #endregion
    }
}