using AutoContext.EntityFrameworkCore.Mapping;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    [MappingConfiguration(contextType: typeof(CatalogContext))]
    public class CatalogEntityTypeConfiguration<TEntity> : AbstractEntityTypeConfiguration<TEntity> where TEntity : class
    {
        
    }
}