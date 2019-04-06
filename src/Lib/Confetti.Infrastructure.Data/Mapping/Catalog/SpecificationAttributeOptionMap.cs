using Confetti.Domain.Core.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a specification attribute option mapping configuration
    /// </summary>
    public partial class SpecificationAttributeOptionMap : ConfettiEntityTypeConfiguration<SpecificationAttributeOption>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<SpecificationAttributeOption> builder)
        {
            builder.ToTable(nameof(SpecificationAttributeOption));
            builder.HasKey(option => option.Id);

            builder.Property(option => option.Name).HasMaxLength(255).IsRequired();

            builder.HasOne(option => option.SpecificationAttribute)
                .WithMany(attribute => attribute.SpecificationAttributeOptions)
                .HasForeignKey(option => option.SpecificationAttributeId)
                .IsRequired();

            base.Configure(builder);
        }

        #endregion
    }
}