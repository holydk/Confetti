using System;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore
{
    public abstract class ConfettiContextFactory<TContext> : IDesignTimeDbContextFactory<TContext> 
        where TContext: ConfettiContext
    {
        #region Properties
            
        /// <summary>
        /// Gets mapping assembly field name contains in <see cref="ConfettiContextFactory.DataSettingsFileName" /> "ConnectionStrings" section.
        /// Default value is MappingAssemblyName.
        /// <remarks>
        /// If migrations assembly field name is not found than mapping assembly is entry assembly.
        /// </remarks>
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
        /// Create context.
        /// </summary>
        /// <param name="args"> The args. </param>
        /// <returns> New instance of TContext. </returns>
        public virtual TContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TContext>();

            var currentDirectory = Path.Combine(Directory.GetCurrentDirectory());
            var configuration = new ConfigurationBuilder()
                .SetBasePath(currentDirectory)
                .AddJsonFile(DataSettingsFileName, optional: false)
                .Build();          

            ConfigureDbContextOptions(configuration, optionsBuilder);

            return (TContext)Activator.CreateInstance(typeof(TContext), optionsBuilder.Options);
        }

        /// <summary>
        /// Configure context options.
        /// </summary>
        /// <param name="configuration"> The configuration. </param>
        /// <param name="builder"> The options builder. </param>
        protected virtual void ConfigureDbContextOptions(
            IConfiguration configuration, 
            DbContextOptionsBuilder<TContext> builder)
        {
            var mappingAssemblyName = configuration.GetValue<string>(MappingAssemblyFieldName, null);
            builder.UseMappingAssembly(mappingAssemblyName ?? Assembly.GetEntryAssembly().FullName);
        }
            
        #endregion
    }
}