using Confetti.Data.Configuration;
using Confetti.Data.Services;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a confetti data extensions for <see cref="ConfettiDataBuilder"/>.
    /// </summary>
    public static class ConfettiDataBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddRequiredPlatformServices(this ConfettiDataBuilder builder)
        {
            builder.Services.AddOptions();

            return builder;
        }

        /// <summary>
        /// Adds the pluggable services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddPluggableServices(this ConfettiDataBuilder builder)
        {
            builder.Services.TryAddScoped<IDataProviderManager, DefaultDataProviderManager>();

            return builder;
        }
    }
}