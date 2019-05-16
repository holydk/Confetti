using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Confetti.Account.Application.Configuration;
using Confetti.Account.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Confetti.Account.Infrastructure.Identity.Validators
{
    public class DefaultPasswordValidator<TUser> : IPasswordValidator<TUser>
        where TUser : ApplicationUser
    {      
        #region Fields

        private readonly AccountOptions _accountOptions;

        private readonly DefaultIdentityErrorDescriber _describer;

        #endregion

        #region Ctor

        public DefaultPasswordValidator(
            IOptions<AccountOptions> accountOptions,
            DefaultIdentityErrorDescriber describer = null)
        {
            _accountOptions = accountOptions.Value;
            _describer = describer ?? new DefaultIdentityErrorDescriber();
        }

        #endregion

        #region Methods

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            var errors = new List<IdentityError>();

            if (password.Length > _accountOptions.MaxPasswordLength - 1)
            {
                errors.Add(_describer.PasswordTooLong(_accountOptions.MaxPasswordLength));
            }

            return (errors.Count > 0) 
                ? Task.FromResult(IdentityResult.Failed(errors.ToArray()))
                : Task.FromResult(IdentityResult.Success);
        }
            
        #endregion
    }
}