using System.Collections.Generic;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a mapping provider.
    /// </summary>
    public interface IMappingProvider
    {
        /// <summary>
        /// Get mapping configurations.
        /// </summary>
        /// <returns> Enumerable of mapping configurations </returns>
        IEnumerable<IMappingConfiguration> GetMappings();
    }
}