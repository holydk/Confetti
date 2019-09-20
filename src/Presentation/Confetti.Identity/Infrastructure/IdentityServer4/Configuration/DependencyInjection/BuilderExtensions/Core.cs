using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Endpoints;
using IdentityServer4.Hosting;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Builder extension methods for registering core services
    /// </summary>
    public static class IdentityServerBuilderExtensionsCore
    {
        /// <summary>
        /// Adds the confetti pluggable services.
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfettiPluggableServices(this IIdentityServerBuilder builder)
        {
            builder.Services.AddScoped<IEndpointResultService, EndpointResultService>();
            builder.Services.AddTransient<IAuthorizeInteractionResponseGenerator, CustomAuthorizeInteractionResponseGenerator>();

            //builder.AddProfileService<TestProfileService>();

            return builder;
        }
    }
}