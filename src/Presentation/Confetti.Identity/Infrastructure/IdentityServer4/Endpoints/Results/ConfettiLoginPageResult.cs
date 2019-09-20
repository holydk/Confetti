using System.Threading.Tasks;
using Confetti.Identity.Configuration;
using Confetti.Identity.Models;
using Confetti.Identity.Services;
using IdentityServer4.Configuration;
using IdentityServer4.Extensions;
using IdentityServer4.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace IdentityServer4.Endpoints.Results
{
    /// <summary>
    /// Result for login page.
    /// </summary>
    /// /// <seealso cref="IdentityServer4.Hosting.IEndpointResult" />
    public class ConfettiLoginPageResult : IEndpointResult
    {
        #region Fields
        private readonly LoginPageResult _result;
        private IdentityServerOptions _identityServerOptions;
        private ConfettiIdentityOptions _confettiIdentityOptions;
        private ISignInService _signInService;
            
        #endregion

        #region Ctor

        public ConfettiLoginPageResult(LoginPageResult result)
        {
            _result = result;
        }
            
        #endregion

        private void Init(HttpContext context)
        {
            _identityServerOptions = _identityServerOptions ?? context.RequestServices.GetRequiredService<IdentityServerOptions>();
            _confettiIdentityOptions = _confettiIdentityOptions ?? context.RequestServices.GetRequiredService<IOptions<ConfettiIdentityOptions>>().Value;
            _signInService = _signInService ?? context.RequestServices.GetRequiredService<ISignInService>();
        }

        /// <summary>
        /// Executes the result.
        /// </summary>
        /// <param name="context">The HTTP context.</param>
        /// <returns></returns>
        public async Task ExecuteAsync(HttpContext context)
        {
            await _result.ExecuteAsync(context);
            
            Init(context);

            if (context.Response.StatusCode == StatusCodes.Status302Found)
            {
                var redirectUri = context.Response.Headers["Location"][0];
                var query = QueryHelpers.ParseQuery(redirectUri);
                var returnUrl = query.AsNameValueCollection()[0];

                var signInMessage = new SignInMessage()
                {
                    ReturnUrl = returnUrl
                };
                var code = _signInService.CreateSignInMessageCode(signInMessage);

                var loginUrl = _identityServerOptions.UserInteraction.LoginUrl;
                var url = loginUrl.AddQueryString(_confettiIdentityOptions.LoginSignInUrlParameter, code);

                context.Response.RedirectToAbsoluteUrl(url);
            } 
        }
    }
}