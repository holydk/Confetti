namespace Confetti.Identity.Application.Configuration
{
    /// <summary>
    /// Specifies options for password requirements.
    /// </summary>
    public class ConfettiPasswordOptions
    {
        /// <summary>
        /// Gets or sets the maximum length a password must be.
        /// </summary>
        public int MaxPasswordLength { get; set; } = 100;
    }
}