using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Infrastructure.Mapping
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public interface IOrderedMapperProfile
    {
        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        int Order { get; }

        /// <summary>
        /// Configure profile
        /// </summary>
        /// <param name="services">Service collection</param>
        void Configure(IServiceCollection services);
    }
}
