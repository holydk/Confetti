using Microsoft.EntityFrameworkCore;

namespace Confetti.Catalog.Infrastructure.Data.EntityFrameworkCore
{
    /// <summary>
    /// Represents a catalog context.
    /// </summary>
    public class CatalogContext : AutoContext.EntityFrameworkCore.AutoContext
    {
        public CatalogContext(DbContextOptions options) : base(options)
        {
        }
    }
}