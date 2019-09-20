using System.Linq;
using IdentityServer4.ResponseHandling;
using IdentityServer4.Services;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DI extension methods for adding IdentityServer
    /// </summary>
    public static class IdentityServerServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Confetti IdentityServer.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <returns></returns>
        public static IIdentityServerBuilder AddConfettiIdentityServer(this IServiceCollection services)
        {
            // var builder = services.AddIdentityServer(options =>
            // {
            //     options.Events.RaiseErrorEvents = true;
            //     options.Events.RaiseInformationEvents = true;
            //     options.Events.RaiseFailureEvents = true;
            //     options.Events.RaiseSuccessEvents = true;
            // });
            //services.AddTransient<IProfileService, TestProfileService>();

            var builder = services.AddIdentityServer();
                //.AddProfileService<TestProfileService>();

            builder
                .AddConfettiPluggableServices();

            return builder;
        }
    }
}