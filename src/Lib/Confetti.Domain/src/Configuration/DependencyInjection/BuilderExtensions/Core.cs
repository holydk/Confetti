using Confetti.Domain.Configuration;
using Confetti.Domain.Uow;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a extensions for <see cref="ConfettiDomainBuilder"/>.
    /// </summary>
    public static class ConfettiDomainBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the required platform services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddRequiredPlatformServices(this ConfettiDomainBuilder builder)
        {
            builder.Services.AddOptions();

            return builder;
        }

        /// <summary>
        /// Adds the pluggable services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddPluggableServices(this ConfettiDomainBuilder builder)
        {
            builder.Services.TryAddTransient<IConnectionStringResolver, DefaultConnectionStringResolver>();
            builder.Services.TryAddScoped<IUnitOfWorkManager, DefaultUnitOfWorkManager>();

            return builder;
        }
    }
}