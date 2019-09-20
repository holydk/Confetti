using System;
using IdentityServer4.Endpoints.Results;
using IdentityServer4.Hosting;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Represents a IdentityServer result factory.
    /// </summary>
    public class EndpointResultService : IEndpointResultService
    {
        /// <summary>
        /// Replace result.
        /// </summary>
        /// <param name="result">The endpoint result.</param>
        public void Replace(ref IEndpointResult result)
        {
            if (result == null) throw new ArgumentNullException(nameof(result));

            switch (result)
            {
                case LoginPageResult loginResult:

                    result = new ConfettiLoginPageResult(loginResult);
                    break;

                default:
                    break;
            }
        }
    }
}