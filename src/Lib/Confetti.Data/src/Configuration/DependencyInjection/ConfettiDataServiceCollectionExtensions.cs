using System;
using Confetti.Data.Configuration;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents a confetti data extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ConfettiDataServiceCollectionExtensions
    {
        /// <summary>
        /// Creates a confetti data builder.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddConfettiDataBuilder(this IServiceCollection services)
        {
            return new ConfettiDataBuilder(services);
        }

        /// <summary>
        /// Adds a confetti data.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddConfettiData(this IServiceCollection services)
        {
            var builder = services.AddConfettiDataBuilder();

            builder
                .AddRequiredPlatformServices()
                .AddPluggableServices();

            return builder;
        }

        /// <summary>
        /// Adds a confetti data.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="setupAction">The setup action.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddConfettiData(this IServiceCollection services, Action<ConfettiDataOptions> setupAction)
        {
            services.Configure(setupAction);
            return services.AddConfettiData();
        }

        /// <summary>
        /// Adds a confetti data.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns></returns>
        public static ConfettiDataBuilder AddConfettiData(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ConfettiDataOptions>(configuration);
            return services.AddConfettiData();
        }
    }
}