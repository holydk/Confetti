using System.Threading.Tasks;
using Confetti.Account.Application.Models;
using Confetti.Common;

namespace Confetti.Account.Application
{
    /// <summary>
    /// Represents a account service.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// Register user.
        /// </summary>
        /// <param name="model">Input model.</param>
        /// <returns>Action result.</returns>
        Task<ActionResult> RegisterAsync(RegisterInputModel model);

        /// <summary>
        /// Validate user credentials.
        /// </summary>
        /// <param name="email">User email.</param>
        /// <param name="password">User password.</param>
        /// <returns>Action result.</returns>
        Task<ActionResult> ValidateCredentialsAsync(string email, string password);
    }
}