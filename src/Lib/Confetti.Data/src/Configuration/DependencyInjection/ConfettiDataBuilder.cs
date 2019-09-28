using System;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Data.Configuration
{
    /// <summary>
    /// Represents a confetti data helper class for DI configuration.
    /// </summary>
    public class ConfettiDataBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfettiDataBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public ConfettiDataBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}