using System;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents database context model mapping configuration
    /// </summary>
    public partial interface IMappingConfiguration
    {
        /// <summary>
        /// The type of context that this configuration are for.
        /// </summary>
        Type ContextType { get; }

        /// <summary>
        /// Apply this mapping configuration
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for the database context</param>
        void ApplyConfiguration(ModelBuilder modelBuilder);
    }
}