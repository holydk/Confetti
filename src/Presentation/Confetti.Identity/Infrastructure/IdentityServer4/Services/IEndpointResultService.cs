using IdentityServer4.Hosting;

namespace IdentityServer4.Services
{
    /// <summary>
    /// Represents a IdentityServer result factory.
    /// </summary>
    public interface IEndpointResultService
    {
        /// <summary>
        /// Replace result.
        /// </summary>
        /// <param name="result">The endpoint result.</param>
        void Replace(ref IEndpointResult result);
    }
}