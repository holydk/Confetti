using System;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Hosting;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.Hosting
{
    /// <summary>
    /// IdentityServer middleware
    /// </summary>
    public class ConfettiIdentityServerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityServerMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        public ConfettiIdentityServerMiddleware(RequestDelegate next, ILogger<IdentityServerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="router">The router.</param>
        /// <param name="session">The user session.</param>
        /// <param name="events">The event service.</param>
        /// <returns></returns>
        public async Task Invoke(
            HttpContext context,
            IEndpointRouter router, 
            IUserSession session, 
            IEventService events,
            IEndpointResultService endpointResultService)
        {
            // this will check the authentication session and from it emit the check session
            // cookie needed from JS-based signout clients.
            await session.EnsureSessionIdCookieAsync();

            try
            {
                var endpoint = router.Find(context);
                if (endpoint != null)
                {
                    _logger.LogInformation("Invoking IdentityServer endpoint: {endpointType} for {url}", endpoint.GetType().FullName, context.Request.Path.ToString());

                    var result = await endpoint.ProcessAsync(context);

                    if (result != null)
                    {
                        endpointResultService.Replace(ref result);
                        _logger.LogTrace("Invoking result: {type}", result.GetType().FullName);
                        var user = await session.GetUserAsync();
                        await result.ExecuteAsync(context);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                await events.RaiseAsync(new UnhandledExceptionEvent(ex));
                _logger.LogCritical(ex, "Unhandled exception: {exception}", ex.Message);
                throw;
            }

            await _next(context);
        }
    }
}