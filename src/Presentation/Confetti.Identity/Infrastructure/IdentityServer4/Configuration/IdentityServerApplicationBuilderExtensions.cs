using IdentityServer4.Hosting;

namespace Microsoft.AspNetCore.Builder
{
    /// <summary>
    /// Pipeline extension methods for adding IdentityServer
    /// </summary>
    public static class IdentityServerApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds IdentityServer to the pipeline.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <returns></returns>
        public static IApplicationBuilder UseConfettiIdentityServer(this IApplicationBuilder app)
        {
            app.UseMiddleware<BaseUrlMiddleware>();

            app.ConfigureCors();

            // it seems ok if we have UseAuthentication more than once in the pipeline --
            // this will just re-run the various callback handlers and the default authN 
            // handler, which just re-assigns the user on the context. claims transformation
            // will run twice, since that's not cached (whereas the authN handler result is)
            // related: https://github.com/aspnet/Security/issues/1399
            app.UseAuthentication();

            app.UseMiddleware<MutualTlsTokenEndpointMiddleware>();
            app.UseMiddleware<ConfettiIdentityServerMiddleware>();

            return app;
        }
    }
}