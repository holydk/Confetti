using Confetti.Infrastructure.Data.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Microsoft.EntityFrameworkCore
{
    /// <summary>
    /// SQL Server specific extension methods for <see cref="DbContextOptionsBuilder" />.
    /// </summary>
    public static class SqlServerDbContextOptionsExtensions
    {
        /// <summary>
        /// Configures source of the mapping configurations from assembly.
        /// </summary>
        /// <param name="optionsBuilder"> The builder being used to configure the context. </param>
        /// <param name="connectionString"> The name of assembly that contains mapping configurations. </param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseMappingAssembly(
            this DbContextOptionsBuilder optionsBuilder,
            string mappingAssembly)
        {
            if (optionsBuilder is null)
            {
                throw new System.ArgumentNullException(nameof(optionsBuilder));
            }

            if (mappingAssembly is null)
            {
                throw new System.ArgumentNullException(nameof(mappingAssembly));
            }

            var extension = (ConfettiOptionsExtension)GetOrCreateExtension(optionsBuilder).WithMappingAssembly(mappingAssembly);
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            return optionsBuilder;
        }

        private static ConfettiOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.Options.FindExtension<ConfettiOptionsExtension>()
               ?? new ConfettiOptionsExtension();
    }
}