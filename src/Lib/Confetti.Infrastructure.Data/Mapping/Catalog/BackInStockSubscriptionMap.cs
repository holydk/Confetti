using Confetti.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a back in stock subscription mapping configuration
    /// </summary>
    public partial class BackInStockSubscriptionMap : ConfettiEntityTypeConfiguration<BackInStockSubscription>
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
                subscription.CustomerProfileId
            });

            // Relations
            builder.HasOne(subscription => subscription.Product)
                .WithMany()
                .HasForeignKey(subscription => subscription.ProductId)
                .IsRequired();

            builder.HasOne(subscription => subscription.CustomerProfile)
                .WithMany()
                .HasForeignKey(subscription => subscription.CustomerProfileId)
                .IsRequired();

            // Indexes
            builder.HasIndex(subscription => subscription.ProductId);
            builder.HasIndex(subscription => subscription.CustomerProfileId);

            base.Configure(builder);
        }

        #endregion
    }
}