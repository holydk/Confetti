using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;

namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides an default implementation of the connection string resolver from <see cref="IConfiguration"/>.
    /// </summary>
    public class DefaultConnectionStringResolver : IConnectionStringResolver
    {
        #region Fields

        private readonly IConfiguration _configuration;
            
        #endregion

        #region Ctor

        public DefaultConnectionStringResolver(IConfiguration configuration)
        {
            _configuration = configuration;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string.
        /// </summary>
        /// <param name="args">Arguments that can be used while resolving connection string.</param>
        /// <returns>The name or connection string.</returns>
        public virtual string GetNameOrConnectionString(IDictionary<string, object> args)
        {
            if (args == null) throw new System.ArgumentNullException(nameof(args));

            var resolved = GetNameOrConnectionString(_configuration, args);

            if (resolved == null)
            {
                var frendlyArgs = string.Empty;
                if (args != null && args.Any())
                {
                    frendlyArgs = string.Join(", ", args.Select(a => $"{a.Key}: {a.Value}"));
                }

                throw new InvalidOperationException(
                    $"Can't resolve a conntection string with args: {frendlyArgs}");
            }

            return resolved;
        }

        /// <summary>
        /// Gets a connection string name (in config file) or a valid connection string.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="args">Arguments that can be used while resolving connection string.</param>
        /// <returns>The name or connection string.</returns>
        protected virtual string GetNameOrConnectionString(IConfiguration configuration, IDictionary<string, object> args)
        {
            var connectionName = "Default";
            var resolved = configuration?[connectionName]
                              ?? configuration?.GetConnectionString(connectionName);

            return resolved != null 
                      ? connectionName
                      : null;
        }
            
        #endregion
    }
}