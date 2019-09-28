using System;
using Confetti.Domain.Configuration;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ConfettiDomainServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a confetti domain builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddConfettiDomainBuilder(this IServiceCollection services)
        {
            return new ConfettiDomainBuilder(services);
        }

        /// <summary>
        /// Adds a confetti domain.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddConfettiDomain(this IServiceCollection services)
        {
            var builder = services.AddConfettiDomainBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddPluggableServices();

            return builder;
        }

        /// <summary>
        /// Adds a confetti domain.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddConfettiDomain(this IServiceCollection services, Action<ConfettiDomainOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddConfettiDomain();
        }

        /// <summary>
        /// Adds a confetti domain.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static ConfettiDomainBuilder AddConfettiDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfettiDomainOptions>(configuration);
            return services.AddConfettiDomain();
        }
    }
}