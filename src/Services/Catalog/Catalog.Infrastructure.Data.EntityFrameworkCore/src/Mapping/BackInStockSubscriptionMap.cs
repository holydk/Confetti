using Confetti.Catalog.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a back in stock subscription mapping configuration
    /// </summary>
    public partial class BackInStockSubscriptionMap : CatalogEntityTypeConfiguration<BackInStockSubscription>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<BackInStockSubscription> builder)
        {
            builder.ToTable(nameof(BackInStockSubscription));
            builder.HasKey(subscription => new 
            {
                subscription.ProductId,
                subscription.CustomerId
            });

            // Relations
            builder.HasOne(subscription => subscription.Product)
                .WithMany()
                .HasForeignKey(subscription => subscription.ProductId)
                .IsRequired();

            // Indexes
            builder.HasIndex(subscription => subscription.ProductId);
            builder.HasIndex(subscription => subscription.CustomerId);

            base.Configure(builder);
        }

        #endregion
    }
}