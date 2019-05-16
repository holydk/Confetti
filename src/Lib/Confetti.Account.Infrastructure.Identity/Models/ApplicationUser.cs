using Microsoft.AspNetCore.Identity;

namespace Confetti.Account.Infrastructure.Identity.Models
{
    /// <summary>
    /// Represents a application user.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Gets of sets user first name.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets of sets user last name.
        /// </summary>
        public string LastName { get; set; }
    }
}