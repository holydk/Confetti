using System;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Domain.Configuration
{
    /// <summary>
    /// Represents a confetti domain helper class for DI configuration.
    /// </summary>
    public class ConfettiDomainBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfettiDomainBuilder"/> class.
        /// </summary>
        /// <param name="services">The services.</param>
        /// <exception cref="System.ArgumentNullException">services</exception>
        public ConfettiDomainBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}