using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Confetti.Account.Application.Configuration;
using Confetti.Account.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Confetti.Account.Infrastructure.Identity.Validators
{
    public class DefaultUserValidator<TUser> : IUserValidator<TUser>
        where TUser : ApplicationUser
    {
        #region Fields

        private readonly AccountOptions _accountOptions;

        private readonly DefaultIdentityErrorDescriber _describer;

        #endregion

        #region Ctor

        public DefaultUserValidator(
            IOptions<AccountOptions> accountOptions,
            DefaultIdentityErrorDescriber describer = null)
        {
            _accountOptions = accountOptions.Value;
            _describer = describer ?? new DefaultIdentityErrorDescriber();
        }

        #endregion

        #region Methods

        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager == null)
                throw new ArgumentNullException(nameof(manager));
            if (user == null)
                throw new ArgumentNullException(nameof(user));

            var errors = new List<IdentityError>();
            
            if (string.IsNullOrWhiteSpace(user.FirstName))
            {
                errors.Add(_describer.NullOrEmptyFirstName());
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName) && user.FirstName.Length > _accountOptions.MaxFirstNameLength)
            {
                errors.Add(_describer.FirstNameTooLong(_accountOptions.MaxFirstNameLength));
            }

            if (string.IsNullOrWhiteSpace(user.LastName))
            {
                errors.Add(_describer.NullOrEmptyLastName());
            }

            if (!string.IsNullOrWhiteSpace(user.LastName) && user.LastName.Length > _accountOptions.MaxLastNameLength)
            {
                errors.Add(_describer.LastNameTooLong(_accountOptions.MaxLastNameLength));
            }

            if (!string.IsNullOrWhiteSpace(user.FirstName) && !Regex.IsMatch(user.FirstName, _accountOptions.FirstNameInvalidSymbolsRegEx, RegexOptions.CultureInvariant))
            {
                errors.Add(_describer.FirstNameInvalidSymbols(_accountOptions.FirstNameInvalidSymbols));
            }

            if (!string.IsNullOrWhiteSpace(user.LastName) && !Regex.IsMatch(user.LastName, _accountOptions.LastNameInvalidSymbolsRegEx, RegexOptions.CultureInvariant))
            {
                errors.Add(_describer.LastNameInvalidSymbols(_accountOptions.LastNameInvalidSymbols));
            }

            return (errors.Count > 0) 
                ? Task.FromResult(IdentityResult.Failed(errors.ToArray()))
                : Task.FromResult(IdentityResult.Success);
        }
            
        #endregion
    }
}