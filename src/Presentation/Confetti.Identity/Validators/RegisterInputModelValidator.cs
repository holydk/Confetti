using System.Text.RegularExpressions;
using Confetti.Account.Application.Configuration;
using Confetti.Account.Application.Models;
using Confetti.Account.Infrastructure.Identity;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace Confetti.Identity.Validators
{
    public class RegisterInputModelValidator : AbstractValidator<RegisterInputModel>
    {
        private readonly AccountOptions _accountOptions;
        private readonly DefaultIdentityErrorDescriber _describer;
        
        public RegisterInputModelValidator(
            IOptions<AccountOptions> accountOptions,
            DefaultIdentityErrorDescriber describer)
        {
            _accountOptions = accountOptions.Value;
            _describer = describer;

            RuleFor(model => model.Email)
                .NotNull()
                .WithMessage(_describer.NullOrEmptyEmail().Description)
                .NotEmpty()
                .WithMessage(_describer.NullOrEmptyEmail().Description)
                .EmailAddress()
                .WithMessage(_describer.InvalidEmail(string.Empty).Description);
            RuleFor(model => model.FirstName)
                .NotNull()
                .WithMessage(_describer.NullOrEmptyFirstName().Description)
                .NotEmpty()
                .WithMessage(_describer.NullOrEmptyFirstName().Description)
                .MaximumLength(_accountOptions.MaxFirstNameLength)
                .WithMessage(_describer.FirstNameTooLong(_accountOptions.MaxFirstNameLength).Description)
                .Matches(_accountOptions.FirstNameInvalidSymbolsRegEx, RegexOptions.CultureInvariant)
                .WithMessage(_describer.FirstNameInvalidSymbols(_accountOptions.FirstNameInvalidSymbols).Description);
            RuleFor(model => model.LastName)
                .NotNull()
                .WithMessage(_describer.NullOrEmptyLastName().Description)
                .NotEmpty()
                .WithMessage(_describer.NullOrEmptyLastName().Description)
                .MaximumLength(_accountOptions.MaxLastNameLength)
                .WithMessage(_describer.LastNameTooLong(_accountOptions.MaxLastNameLength).Description)
                .Matches(_accountOptions.LastNameInvalidSymbolsRegEx, RegexOptions.CultureInvariant)
                .WithMessage(_describer.LastNameInvalidSymbols(_accountOptions.LastNameInvalidSymbols).Description);
            RuleFor(model => model.Password)
                .NotNull()
                .WithMessage(_describer.NullOrEmptyPassword().Description)
                .NotEmpty()
                .WithMessage(_describer.NullOrEmptyPassword().Description)
                .MaximumLength(_accountOptions.MaxPasswordLength)
                .WithMessage(_describer.PasswordTooLong(_accountOptions.MaxPasswordLength).Description)
                .MinimumLength(_accountOptions.RequiredPasswordLength)
                .WithMessage(_describer.PasswordTooShort(_accountOptions.RequiredPasswordLength).Description);
            RuleFor(model => model.ConfirmPassword)
                .NotNull()
                .WithMessage(_describer.NullOrEmptyPassword().Description)
                .NotEmpty()
                .WithMessage(_describer.NullOrEmptyPassword().Description)
                .Equal(model => model.Password)
                .WithMessage(_describer.PasswordMismatch().Description);
        }
    }
}