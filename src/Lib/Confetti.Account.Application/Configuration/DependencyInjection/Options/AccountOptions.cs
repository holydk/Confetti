using System;

namespace Confetti.Account.Application.Configuration
{
    /// <summary>
    /// Represents a account options.
    /// </summary>
    public class AccountOptions
    {
        /// <summary>
        /// Gets or sets a flag indicating whether the application requires unique emails.
        /// </summary>
        public bool RequireUniqueEmail { get; set; }

        /// <summary>
        /// Gets or sets the maximum length a first name must be.
        /// </summary>
        public int MaxFirstNameLength { get; set; }

        /// <summary>
        /// Gets or sets the invalid symbols of first name.
        /// </summary>
        public string FirstNameInvalidSymbols { get; set; }

        /// <summary>
        /// Gets or sets the regular expression of invalid symbols of first name.
        /// </summary>
        public string FirstNameInvalidSymbolsRegEx { get; set; }

        /// <summary>
        /// Gets or sets the maximum length a last name must be.
        /// </summary>
        public int MaxLastNameLength { get; set; }

        /// <summary>
        /// Gets or sets the invalid symbols of last name.
        /// </summary>
        public string LastNameInvalidSymbols { get; set; }

        /// <summary>
        /// Gets or sets the regular expression of invalid symbols of last name.
        /// </summary>
        public string LastNameInvalidSymbolsRegEx { get; set; }

        /// <summary>
        /// Gets or sets the minimum length a password must be.
        /// </summary>
        public int RequiredPasswordLength { get; set; }

        /// <summary>
        /// Gets or sets the maximum length a password must be.
        /// </summary>
        public int MaxPasswordLength { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a non-alphanumeric character.
        /// </summary>
        public bool RequireNonAlphanumericPassword { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a lower case ASCII character.
        /// </summary>
        public bool RequireLowercasePassword { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a upper case ASCII character.
        /// </summary>
        public bool RequireUppercasePassword { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if passwords must contain a digit.
        /// </summary>
        public bool RequireDigitPassword { get; set; }

        /// <summary>
        /// Gets or sets the number of failed access attempts allowed before a user is locked
        /// out, assuming lock out is enabled.
        /// </summary>
        public int MaxFailedAccessAttempts { get; set; }

        /// <summary>
        /// Gets or sets the System.TimeSpan a user is locked out for when a lockout occurs.
        /// </summary>
        /// <value></value>
        public TimeSpan DefaultLockoutTimeSpan { get; set; }

        public AccountOptions()
        {
            RequireUniqueEmail = true;
            MaxFirstNameLength = 25;
            FirstNameInvalidSymbols = "<, >, &, \" и .";
            FirstNameInvalidSymbolsRegEx = "^(?!.*[<>\\.&\"]).*$";
            MaxLastNameLength = 25;
            LastNameInvalidSymbols = "<, >, &, \" и .";
            LastNameInvalidSymbolsRegEx = "^(?!.*[<>\\.&\"]).*$";
            RequiredPasswordLength = 6;
            MaxPasswordLength = 100;
            RequireNonAlphanumericPassword = false;
            RequireLowercasePassword = false;
            RequireUppercasePassword = false;
            RequireDigitPassword = false;
            MaxFailedAccessAttempts = 3;
            DefaultLockoutTimeSpan = new TimeSpan(0, 5, 0);
        }
    }
}