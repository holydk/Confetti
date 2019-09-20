using System.Threading.Tasks;
using Confetti.Identity.Models;

namespace Confetti.Identity.Services
{
    /// <summary>
    /// Represents a signIn service.
    /// </summary>
    public interface ISignInService
    {
        /// <summary>
        /// Creates code of contextual information about a login request.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <returns>The signIn message code.</returns>
        string CreateSignInMessageCode(SignInMessage message);

        /// <summary>
        /// Gets contextual information about a login request by code.
        /// </summary>
        /// <param name="code">The signIn message code.</param>
        /// <returns>The signIn message.</returns>
        SignInMessage GetSignInMessage(string code);

        /// <summary>
        /// Signs in the specified user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <param name="rememberLogin">Remember login?</param>
        /// <returns></returns>
        Task SignInAsync(string email, bool rememberLogin = false);

        /// <summary>
        /// Signs the current user out of the application.
        /// </summary>
        /// <returns></returns>
        Task SignOutAsync();
    }
}