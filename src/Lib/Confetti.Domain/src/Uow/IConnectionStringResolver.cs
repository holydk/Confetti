using System.Collections.Generic;

namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides an abstraction to get connection string.
    /// </summary>
    public interface IConnectionStringResolver
    {
        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string.
        /// </summary>
        /// <param name="args">Arguments that can be used while resolving connection string.</param>
        string GetNameOrConnectionString(IDictionary<string, object> args);
    }
}