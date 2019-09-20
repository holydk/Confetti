using System;
using System.Threading.Tasks;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Identity.Configuration;
using Confetti.Identity.Infrastructure.Hosting;
using Confetti.Identity.Models;
using Confetti.Identity.Services;
using IdentityServer4.Events;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Confetti.Identity.Infrastructure.Factories
{
    /// <summary>
    /// Represents a signIn service.
    /// </summary>
    public class SignInService : ISignInService
    {
        #region Fields

        private readonly IEventService _events;
        private readonly ConfettiIdentityOptions _options;
        private readonly HttpContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
            
        #endregion

        #region Ctor

        public SignInService(
            IEventService events,
            IOptions<ConfettiIdentityOptions> options,
            IHttpContextAccessor httpContextAccessor,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _events = events;
            _options = options.Value;
            _context = httpContextAccessor.HttpContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Creates code of contextual information about a login request.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <returns>The signIn message code.</returns>
        public string CreateSignInMessageCode(SignInMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));

            var cookie = new MessageCookie<SignInMessage>(_context);
            var code = cookie.Write(message);

            return code;
        }

        /// <summary>
        /// Gets contextual information about a login request by code.
        /// </summary>
        /// <param name="code">The signIn message code.</param>
        /// <returns>The signIn message.</returns>
        public SignInMessage GetSignInMessage(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            var cookie = new MessageCookie<SignInMessage>(_context);
            var message = cookie.Read(code);

            return message;
        }

        /// <summary>
        /// Signs in the specified user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="rememberLogin">Remember login?</param>
        /// <returns></returns>
        public async Task SignInAsync(string email, bool rememberLogin = false)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return;
            }

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Email, user.Id, user.Email));

            // only set explicit expiration here if user chooses "remember me". 
            // otherwise we rely upon expiration configured in cookie middleware.
            AuthenticationProperties props = null;
            if (_options.AllowRememberLogin && rememberLogin)
            {
                props = new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.Add(_options.RememberMeLoginDuration)
                };
            };

            // issue authentication cookie with subject ID and username
            await _signInManager.SignInAsync(user, props, "ASOS");
        }

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <returns></returns>
        public Task SignOutAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}