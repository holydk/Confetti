using System;
using System.Collections.Generic;
using System.Linq;
using Confetti.Data.Configuration;
using Microsoft.Extensions.Options;

namespace Confetti.Data.Services
{
    /// <summary>
    /// Provides an default implementation for a managing of <see cref="IDataProviderHandler"/>'s.
    /// </summary>
    public class DefaultDataProviderManager : IDataProviderManager
    {
        #region Fields

        private readonly IEnumerable<DataProvider> _dataProviders;
        private readonly ConfettiDataOptions _options;
        private readonly IServiceProvider _serviceProvider;

        #endregion

        #region Ctor

        public DefaultDataProviderManager(
            IEnumerable<DataProvider> dataProviders,
            IOptions<ConfettiDataOptions> options,
            IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _dataProviders = dataProviders;
            _serviceProvider = serviceProvider;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Gets a <see cref="IDataProviderHandler"/> by name.
        /// </summary>
        /// <value>The <see cref="IDataProviderHandler"/>.</value>
        public virtual IDataProviderHandler GetDefault()
        {
            return GetByName(_options.DefaultDataProviderName);
        }

        /// <summary>
        /// Gets a default <see cref="IDataProviderHandler"/>.
        /// </summary>
        /// <value>The <see cref="IDataProviderHandler"/>.</value>
        public virtual IDataProviderHandler GetByName(string name)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));

            var provider = _dataProviders.FirstOrDefault(p => p.Name == name);
            if (provider != null)
            {
                return GetHandler(provider);
            }
            
            return null;
        }
            
        #endregion

        #region Utilities

        private IDataProviderHandler GetHandler(DataProvider provider)
        {
            return _serviceProvider.GetService(provider.Handler) as IDataProviderHandler;
        }
            
        #endregion
    }
}