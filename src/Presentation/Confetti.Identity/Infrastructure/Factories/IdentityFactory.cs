using System;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Account.Application.Models;
using Confetti.Identity.Configuration;
using Confetti.Identity.Factories;
using Confetti.Identity.Models;
using Confetti.Identity.ViewModels;
using IdentityServer4;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Confetti.Identity.Infrastructure.Factories
{
    /// <summary>
    /// Represents a identity factory.
    /// </summary>
    public class IdentityFactory : IIdentityFactory
    {
        #region Fields

        private readonly ConfettiIdentityOptions _options;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        #endregion

        #region Ctor

        public IdentityFactory(
            IOptions<ConfettiIdentityOptions> options,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider
        )
        {
            _options = options.Value;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Prepare login view model.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <returns>Login view model.</returns>
        public async Task<LoginViewModel> PrepareLoginViewModelAsync(SignInMessage message)
        {
            if (message == null) throw new ArgumentNullException(nameof(message));
            
            var context = await _interaction.GetAuthorizationContextAsync(message.ReturnUrl);

            if (context?.IdP != null)
            {
                var local = context.IdP == IdentityServerConstants.LocalIdentityProvider;

                // this is meant to short circuit the UI and only trigger the one external IdP
                var vm = new LoginViewModel
                {
                    EnableLocalLogin = local,
                    Email = context?.LoginHint
                };

                if (!local)
                {
                    vm.ExternalProviders = new[] 
                    { 
                        new ExternalProvider 
                        { 
                            AuthenticationScheme = context.IdP 
                        } 
                    };
                }

                return vm;
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();

            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            (x.Name.Equals(_options.WindowsAuthenticationSchemeName, StringComparison.OrdinalIgnoreCase))
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();

            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;

                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider => client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = _options.AllowRememberLogin,
                EnableLocalLogin = allowLocal && _options.AllowLocalLogin,
                Email = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        /// <summary>
        /// Prepare login view model.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <param name="model">login input model.</param>
        /// <returns>login view model.</returns>
        public async Task<LoginViewModel> PrepareLoginViewModelAsync(SignInMessage message, LoginInputModel model)
        {
            var vm = await PrepareLoginViewModelAsync(message);
            vm.Email = model.Email;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        /// <summary>
        /// Prepare register view model.
        /// </summary>
        /// <returns>Register view model.</returns>
        public Task<RegisterViewModel> PrepareRegisterViewModelAsync()
        {
            return Task.FromResult(new RegisterViewModel());
        }
        
        /// <summary>
        /// Prepare register view model.
        /// </summary>
        /// <param name="model">Register input model.</param>
        /// <returns>Register view model.</returns>
        public async Task<RegisterViewModel> PrepareRegisterViewModelAsync(RegisterInputModel model)
        {
            var vm = await PrepareRegisterViewModelAsync();
            vm.Email = model.Email;
            vm.FirstName = model.FirstName;
            vm.LastName = model.LastName;
            return vm;
        }

        #endregion
    }
}