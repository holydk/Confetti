namespace Confetti.Identity.Application.Configuration
{
    /// <summary>
    /// Options for user validation.
    /// </summary>
    public class ConfettiUserOptions
    {
        #region First name

        /// <summary>
        /// Gets or sets the maximum length a first name must be.
        /// </summary>
        public int MaxFirstNameLength { get; set; } = 25;

        /// <summary>
        /// Gets or sets the invalid symbols of first name.
        /// </summary>
        public string FirstNameInvalidSymbols { get; set; } = "<, >, &, \" и .";

        /// <summary>
        /// Gets or sets the regular expression of invalid symbols of first name.
        /// </summary>
        public string FirstNameInvalidSymbolsRegEx { get; set; } = "^(?!.*[<>\\.&\"]).*$";
            
        #endregion

        #region Last name

        /// <summary>
        /// Gets or sets the maximum length a last name must be.
        /// </summary>
        public int MaxLastNameLength { get; set; } = 25;

        /// <summary>
        /// Gets or sets the invalid symbols of last name.
        /// </summary>
        public string LastNameInvalidSymbols { get; set; } = "<, >, &, \" и .";

        /// <summary>
        /// Gets or sets the regular expression of invalid symbols of last name.
        /// </summary>
        public string LastNameInvalidSymbolsRegEx { get; set; } = "^(?!.*[<>\\.&\"]).*$";
            
        #endregion
    }
}