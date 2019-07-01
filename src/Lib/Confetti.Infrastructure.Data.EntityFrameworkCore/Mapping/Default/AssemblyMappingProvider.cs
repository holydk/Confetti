using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore.Mapping
{
    /// <summary>
    /// Represents a assembly mapping provider.
    /// </summary>
    public class AssemblyMappingProvider : IMappingProvider
    {
        #region Fields

        private Assembly _mappingAssembly;
            
        #endregion

        #region Ctor

        public AssemblyMappingProvider(IDbContextOptions options)
        {
            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var assemblyName = ConfettiOptionsExtension.Extract(options)?.MappingAssembly;
            _mappingAssembly = assemblyName == null
                ? Assembly.GetExecutingAssembly()
                : Assembly.Load(new AssemblyName(assemblyName));
        }
            
        #endregion

        #region Utilities
            
        #endregion

        #region Methods

        /// <summary>
        /// Get mapping configurations.
        /// </summary>
        /// <returns> Enumerable of mapping configurations </returns>
        public IEnumerable<IMappingConfiguration> GetMappings()
        {
            // dynamically load all entity and query type configurations
            var typeConfigurations = _mappingAssembly.GetTypes().Where(type =>
            {
                var temp = type;
                do
                {
                    if ((type.GetInterface(nameof(IMappingConfiguration)) != null)
                        && (!type.IsAbstract)
                        && (!type.IsGenericType))
                    {
                        return true;
                    }
                    temp = temp.BaseType;
                } while (temp != null);

                return false;
            })
            .Select(type => (IMappingConfiguration)Activator.CreateInstance(type));

            return typeConfigurations;
        }
            
        #endregion
    }
}