using System;
using System.Linq;
using System.Threading.Tasks;
using Confetti.Account.Application;
using Confetti.Account.Application.Models;
using Confetti.Account.Infrastructure.Identity;
using Confetti.Account.Infrastructure.Identity.Models;
using Confetti.Common;
using Confetti.Common.Security;
using Microsoft.AspNetCore.Identity;

namespace Confetti.Account.Infrastructure
{
    /// <summary>
    /// Represents a account service.
    /// </summary>
    public class AccountService : IAccountService
    {
        #region Fields

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly DefaultIdentityErrorDescriber _describer;

        #endregion

        #region Ctor

        public AccountService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            DefaultIdentityErrorDescriber describer
        )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _describer = describer;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">Input model.</param>
        /// <returns>Action result.</returns>
        public async Task<ActionResult> RegisterAsync(RegisterInputModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var result = new ActionResult();

            if (string.IsNullOrWhiteSpace(model.Password))
            {
                AddIdentityError(result, _describer.NullOrEmptyPassword());
                return result;
            }

            if (string.IsNullOrWhiteSpace(model.ConfirmPassword))
            {
                AddIdentityError(result, _describer.NullOrEmptyConfirmPassword());
                return result;
            }

            if (model.Password != model.ConfirmPassword)
            {
                AddIdentityError(result, _describer.PasswordMismatch());
                return result;
            }
            
            var user = new ApplicationUser()
            {
                Email = model.Email,
                UserName = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var identityResult = await _userManager.CreateAsync(user, model.Password);
            if (!identityResult.Succeeded)
            {
                AddIdentityErrors(result, identityResult);
                return result;
            }

            user = await _userManager.FindByEmailAsync(user.Email);

            // user is created
            // add user to role
            identityResult = await _userManager.AddToRoleAsync(user, DefaultGlobalRoles.Customers);
            if (!identityResult.Succeeded)
            {
                AddIdentityErrors(result, identityResult);
                return result;
            }

            return ActionResult.Success;
        }

        /// <summary>
        /// Validate user credentials.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Action result.</returns>
        public async Task<ActionResult> ValidateCredentialsAsync(string email, string password)
        {
            var result = new ActionResult();

            if (string.IsNullOrWhiteSpace(email))
            {
                AddIdentityError(result, _describer.NullOrEmptyEmail());
                return result;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                AddIdentityError(result, _describer.NullOrEmptyPassword());
                return result;
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                AddIdentityError(result, _describer.UserNotFound());
                return result;
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!validPassword)
            {
                AddIdentityError(result, _describer.InvalidPassword());
                return result;
            }

            return ActionResult.Success;
        }
            
        #endregion

        #region Utilities

        private void AddIdentityErrors(ActionResult result, IdentityResult identityResult)
        {
            foreach (var error in identityResult.Errors)
            {
                AddIdentityError(result, error);
            }
        }

        private void AddIdentityError(ActionResult result, IdentityError identityError)
        {
            if (int.TryParse(identityError.Code, out int code))
            {
                result.AddError(identityError.Description, code);
            }
            else
            {
                result.AddError(identityError.Description);
            }
        }
            
        #endregion
    }
}