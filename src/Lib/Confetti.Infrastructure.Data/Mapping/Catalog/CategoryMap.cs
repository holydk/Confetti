using Confetti.Domain.Catalog;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Confetti.Infrastructure.Data.Mapping.Catalog
{
    /// <summary>
    /// Represents a category mapping configuration
    /// </summary>
    public partial class CategoryMap : ConfettiEntityTypeConfiguration<Category>
    {
        #region Methods

        /// <summary>
        /// Configures the entity
        /// </summary>
        /// <param name="builder">The builder to be used to configure the entity</param>
        public override void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable(nameof(Category));
            builder.HasKey(category => category.Id);

            builder.Property(category => category.Title).HasMaxLength(400).IsRequired();
            builder.Property(category => category.MetaKeywords).HasMaxLength(400);
            builder.Property(category => category.MetaTitle).HasMaxLength(400);

            // Indexes
            builder.HasIndex(c => c.ParentId);
            builder.HasIndex(c => c.Position);
            builder.HasIndex(c => c.Deleted);

            base.Configure(builder);
        }

        #endregion
    }
}