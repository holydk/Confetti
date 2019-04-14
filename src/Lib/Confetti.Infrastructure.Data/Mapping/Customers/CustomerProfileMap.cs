using Confetti.Domain.Customers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Customers
{
    /// <summary>
    /// Represents a customer profile mapping configuration
    /// </summary>
    public partial class CustomerProfileMap : ConfettiEntityTypeConfiguration<CustomerProfile>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<CustomerProfile> builder)
        {
            builder.ToTable(nameof(CustomerProfile));
            builder.HasKey(profile => profile.SubjectId);

            base.Configure(builder);
        }
            
        #endregion
    }
}