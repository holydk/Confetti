using System;
using System.Linq;
using System.Reflection;
using Confetti.Infrastructure.Data.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Data
{
    /// <summary>
    /// Represents base object context
    /// </summary>
    public partial class ConfettiContext : DbContext
    {
        #region Ctor

        public ConfettiContext(DbContextOptions<ConfettiContext> options)
            : base(options)
        {           
        }
            
        #endregion

        #region Utilities

        /// <summary>
        /// Further configuration the model
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // dynamically load all entity and query type configurations
            var typeConfigurations = Assembly.GetExecutingAssembly().GetTypes().Where(type => 
                (type.BaseType?.IsGenericType ?? false) 
                    && (type.BaseType.GetGenericTypeDefinition() == typeof(ConfettiEntityTypeConfiguration<>) 
                        || type.BaseType.GetGenericTypeDefinition() == typeof(ConfettiQueryTypeConfiguration<>)));

            foreach (var typeConfiguration in typeConfigurations)
            {
                var configuration = (IMappingConfiguration)Activator.CreateInstance(typeConfiguration);
                configuration.ApplyConfiguration(modelBuilder);
            }
            
            base.OnModelCreating(modelBuilder);
        }
            
        #endregion
    }
}