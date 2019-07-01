using Microsoft.EntityFrameworkCore;

namespace Confetti.Infrastructure.Data.EntityFrameworkCore
{
    /// <summary>
    /// Represents base object context
    /// </summary>
    public class ConfettiContext : DbContext
    {
        #region Ctor

        public ConfettiContext(DbContextOptions options)
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
            foreach (var configuration in this.GetMappings())
            {
                if (configuration.ContextType == GetType())
                    configuration.ApplyConfiguration(modelBuilder);
            }
            
            base.OnModelCreating(modelBuilder);
        }
            
        #endregion
    }
}