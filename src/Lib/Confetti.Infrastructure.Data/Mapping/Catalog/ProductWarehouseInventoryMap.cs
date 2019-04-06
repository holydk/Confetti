using Confetti.Domain.Core.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a product warehouse inventory mapping configuration
    /// </summary>
    public partial class ProductWarehouseInventoryMap : ConfettiEntityTypeConfiguration<ProductWarehouseInventory>
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

            builder.HasOne(productWarehouseInventory => productWarehouseInventory.Warehouse)
                .WithMany()
                .HasForeignKey(productWarehouseInventory => productWarehouseInventory.WarehouseId)
                .IsRequired();

            // Indexes
            builder.HasIndex(productWarehouseInventory => productWarehouseInventory.ProductId);
            builder.HasIndex(productWarehouseInventory => productWarehouseInventory.WarehouseId);

            base.Configure(builder);
        }

        #endregion
    }
}