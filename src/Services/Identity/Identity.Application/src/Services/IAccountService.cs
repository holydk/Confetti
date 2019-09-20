using System.Threading.Tasks;
using Confetti.Identity.Application.Models;

namespace Confetti.Identity.Application.Services
{
    /// <summary>
    /// Provides the APIs for account managing.
    /// </summary>
    public interface IAccountService<TUser> where TUser : class
    {
        /// <summary>
        /// Finds user by email.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>The user.</returns>
        Task<TUser> FindByEmailAsync(string email);

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The request for registration of new user.</param>
        /// <returns>The result of register operation.</returns>
        Task<RegisterResult> RegisterAsync(RegisterRequest request);

        /// <summary>
        /// Validates a user credentials.
        /// </summary>
        /// <param name="user">The user whose credentials should be validated.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The result of credentials verification operation.</returns>
        Task<ValidateCredentialsResult> ValidateCredentialsAsync(TUser user, string password);
    }
}