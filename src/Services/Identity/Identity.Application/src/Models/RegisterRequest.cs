namespace Confetti.Identity.Application.Models
{
    /// <summary>
    /// Represents a request for registration of new user.
    /// </summary>
    public class RegisterRequest
    {
        /// <summary>
        /// Gets or sets user email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets first name of user.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name of user.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets password confirmation.
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}