using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Account.Application;
using Confetti.Account.Application.Models;
using Confetti.Common;
using Confetti.Identity.Configuration;
using Confetti.Identity.Factories;
using Confetti.Identity.Services;
using Confetti.Identity.ViewModels;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Confetti.Identity.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        #region Fields

        private readonly ConfettiIdentityOptions _options;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IEventService _events;
        private readonly IAccountService _accountService;
        private readonly ISignInService _signInService;
        private readonly IIdentityFactory _identityFactory;

        #endregion

        #region Ctor

        public AccountController(
            IOptions<ConfettiIdentityOptions> options,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IEventService events,
            IAccountService accountService,
            ISignInService signInService,
            IIdentityFactory identityFactory
        )
        {
            _options = options.Value;
            _interaction = interaction;
            _clientStore = clientStore;
            _events = events;
            _accountService = accountService;
            _signInService = signInService;
            _identityFactory = identityFactory;
        }

        #endregion

        #region Methods

        #region Register

        /// <summary>
        /// Entry point into the register workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Register(string signin)
        {
            var signInMessage = _signInService.GetSignInMessage(signin);
            if (signInMessage == null)
            {
                return Redirect(_options.DefaultReturnUrl);
            }

            var model = await _identityFactory.PrepareRegisterViewModelAsync();
            return View(model);
        }

        /// <summary>
        /// Handle postback from register
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([FromQuery] string signin, RegisterInputModel model)
        {
            var signInMessage = _signInService.GetSignInMessage(signin);
            if (signInMessage == null)
            {
                return Redirect(_options.DefaultReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var result = await _accountService.RegisterAsync(model);
                if (result.Succeeded)
                {
                    // send email verification
                    await _signInService.SignInAsync(model.Email);

                    var context = await _interaction.GetAuthorizationContextAsync(signInMessage.ReturnUrl);
                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View(
                                "Redirect", 
                                new RedirectViewModel 
                                { 
                                    RedirectUrl = signInMessage.ReturnUrl
                                });
                        }

                        // we can trust returnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(signInMessage.ReturnUrl);
                    }

                    // invalid returnUrl
                    // no local redirection
                    return Redirect(_options.DefaultReturnUrl);
                }

                AddErrorsToModelState(result.Errors);
            }

            // something went wrong, show form with error
            return View(await _identityFactory.PrepareRegisterViewModelAsync(model));
        }
            
        #endregion

        #region Log In

        /// <summary>
        /// Entry point into the login workflow
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string signin)
        {
            var signInMessage = _signInService.GetSignInMessage(signin);
            if (signInMessage == null)
            {
                return Redirect(_options.DefaultReturnUrl);
            }

            // build a model so we know what to show on the login page
            var model = await _identityFactory.PrepareLoginViewModelAsync(signInMessage);

            // if (vm.IsExternalLoginOnly)
            // {
            //     // we only have one option for logging in and it's an external provider
            //     return RedirectToAction(
            //         "Challenge", 
            //         "External", 
            //         new 
            //         { 
            //             provider = vm.ExternalLoginScheme, returnUrl 
            //         }
            //     );
            // }

            return View(model);
        }

        /// <summary>
        /// Handle postback from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromQuery] string signin, LoginInputModel model)
        {
            var signInMessage = _signInService.GetSignInMessage(signin);
            if (signInMessage == null)
            {
                return Redirect(_options.DefaultReturnUrl);
            }

            if (ModelState.IsValid)
            {
                var result = await _accountService.ValidateCredentialsAsync(model.Email, model.Password);
                if (result.Succeeded)
                {                   
                    // issue authentication cookie with subject ID and username
                    await _signInService.SignInAsync(model.Email, model.RememberLogin);

                    var context = await _interaction.GetAuthorizationContextAsync(signInMessage.ReturnUrl);
                    if (context != null)
                    {
                        if (await _clientStore.IsPkceClientAsync(context.ClientId))
                        {
                            // if the client is PKCE then we assume it's native, so this change in how to
                            // return the response is for better UX for the end user.
                            return View(
                                "Redirect", 
                                new RedirectViewModel 
                                { 
                                    RedirectUrl = signInMessage.ReturnUrl
                                });
                        }

                        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
                        return Redirect(signInMessage.ReturnUrl);
                    }

                    // invalid returnUrl
                    // no local redirection
                    return Redirect(_options.DefaultReturnUrl);
                }

                await _events.RaiseAsync(new UserLoginFailureEvent(model.Email, "invalid credentials"));
                AddErrorsToModelState(result.Errors);
            }

            // something went wrong, show form with error
            return View(await _identityFactory.PrepareLoginViewModelAsync(signInMessage, model));
        }

        #endregion
            
        #endregion

        #region Utilities

        private void AddErrorsToModelState(IEnumerable<ActionError> errors)
        {
            foreach (var error in errors)
            {
                ModelState.AddModelError(
                    error.Code.HasValue 
                        ? error.Code.ToString()
                        : string.Empty, 
                    error.Message);
            }
        }
            
        #endregion
    }
}