using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Confetti.Infrastructure.EntityFrameworkCore.Design
{
    /// <summary>
    /// Represents a abstractions for database context factory.
    /// Note: The database context should contain the constructor with DbContextOptions.
    /// </summary>
    /// <typeparam name="TContext">The type of database context.</typeparam>
    public abstract class ConfettiContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> where TContext: DbContext
    {
        #region Properties
            
        /// <summary>
        /// Gets mapping assembly field name contains in <see cref="DataSettingsFileName" />.
        /// Default value is MappingAssemblyName.
        /// </summary>
        protected virtual string MappingAssemblyFieldName => "MappingAssemblyName";

        /// <summary>
        /// Gets data settings filename.
        /// Default value is datasettings.json.
        /// </summary>
        protected virtual string DataSettingsFileName => "datasettings.json";

        #endregion

        #region Methods

        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns>New instance of <see cref="TContext" />.</returns>
        public virtual TContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(DataSettingsFileName, optional: false)
                .Build();          

            ConfigureDbContextOptions(configuration, optionsBuilder);

            return CreateDbContext(optionsBuilder);
        }

        /// <summary>
        /// Configures the context options from configuration.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="builder">The options builder.</param>
        protected virtual void ConfigureDbContextOptions(IConfiguration configuration, DbContextOptionsBuilder<TContext> builder)
        {
            var mappingAssemblyName = configuration.GetValue<string>(MappingAssemblyFieldName, null);
            builder.UseMappingAssembly(mappingAssemblyName);
        }

        /// <summary>
        /// Creates the database context.
        /// </summary>
        /// <param name="builder">The options builder.</param>
        /// <returns>New instance of <see cref="TContext" />.</returns>
        protected virtual TContext CreateDbContext(DbContextOptionsBuilder<TContext> builder)
        {
            return (TContext)Activator.CreateInstance(typeof(TContext), builder.Options);
        }
            
        #endregion
    }
}