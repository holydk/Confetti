using System;
using System.Threading.Tasks;
using Confetti.Identity.Application.Models;
using Confetti.Identity.Application.Services;
using Confetti.Identity.Infrastructure.Application.AspNetIdentity;

namespace Confetti.Identity.Infrastructure.Application.Services
{
    /// <summary>
    /// Provides a default implementation the APIs for account managing.
    /// </summary>
    public class AccountService : IAccountService<ApplicationUser>
    {
        #region Fields



        #endregion

        #region Ctor

        public AccountService()
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Finds user by email.
        /// </summary>
        /// <param name="email">The email address to return the user for.</param>
        /// <returns>The user.</returns>
        public Task<ApplicationUser> FindByEmailAsync(string email)
        {
            if (email == null) throw new ArgumentNullException(nameof(email));

            
            
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="request">The request for registration of new user.</param>
        /// <returns>The result of register operation.</returns>
        public Task<RegisterResult> RegisterAsync(RegisterRequest request)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Validates a user credentials.
        /// </summary>
        /// <param name="user">The user whose credentials should be validated.</param>
        /// <param name="password">The user password.</param>
        /// <returns>The result of credentials verification operation.</returns>
        public Task<ValidateCredentialsResult> ValidateCredentialsAsync(ApplicationUser user, string password)
        {
            throw new System.NotImplementedException();
        }
            
        #endregion
    }
}