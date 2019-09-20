using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IdentityServer4.ResponseHandling
{
    /// <summary>
    /// IProfileService to integrate with ASP.NET Identity.
    /// </summary>
    /// <typeparam name="TUser">The type of the user.</typeparam>
    /// <seealso cref="IdentityServer4.Services.IProfileService" />
    public class TestProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = new List<Claim>
            {
                new Claim("FullName", "test"),
            };

            context.IssuedClaims.AddRange(claims);
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            // context.IsActive = true;
            return Task.CompletedTask;
        }
    }

    public class CustomAuthorizeInteractionResponseGenerator : AuthorizeInteractionResponseGenerator
    {
        /// <summary>
        /// The HTTP context accessor
        /// </summary>
        protected readonly IHttpContextAccessor HttpContextAccessor;

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        protected HttpContext HttpContext => HttpContextAccessor.HttpContext;

        public CustomAuthorizeInteractionResponseGenerator(
            ISystemClock clock, 
            ILogger<AuthorizeInteractionResponseGenerator> logger, 
            IConsentService consent, 
            IProfileService profile,
            IHttpContextAccessor httpContextAccessor) 
            : base(clock, logger, consent, profile)
        {
            HttpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Processes the interaction logic.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        public override Task<InteractionResponse> ProcessInteractionAsync(
            ValidatedAuthorizeRequest request, 
            ConsentResponse consent = null)
        {
            var anon_acr_value = request.GetPrefixedAcrValue("0");
            return base.ProcessInteractionAsync(request, consent);
        }
    }
}