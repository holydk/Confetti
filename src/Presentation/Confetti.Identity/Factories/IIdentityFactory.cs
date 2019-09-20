using System.Threading.Tasks;
using Confetti.Account.Application.Models;
using Confetti.Identity.Models;
using Confetti.Identity.ViewModels;

namespace Confetti.Identity.Factories
{
    /// <summary>
    /// Represents a identity factory.
    /// </summary>
    public interface IIdentityFactory
    {
        /// <summary>
        /// Prepare login view model.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <returns>Login view model.</returns>
        Task<LoginViewModel> PrepareLoginViewModelAsync(SignInMessage message);

        /// <summary>
        /// Prepare login view model.
        /// </summary>
        /// <param name="message">The signIn message.</param>
        /// <param name="model">login input model.</param>
        /// <returns>login view model.</returns>
        Task<LoginViewModel> PrepareLoginViewModelAsync(SignInMessage message, LoginInputModel model);

        /// <summary>
        /// Prepare register view model.
        /// </summary>
        /// <returns>Register view model.</returns>
        Task<RegisterViewModel> PrepareRegisterViewModelAsync();
        
        /// <summary>
        /// Prepare register view model.
        /// </summary>
        /// <param name="model">Register input model.</param>
        /// <returns>Register view model.</returns>
        Task<RegisterViewModel> PrepareRegisterViewModelAsync(RegisterInputModel model);
    }
}