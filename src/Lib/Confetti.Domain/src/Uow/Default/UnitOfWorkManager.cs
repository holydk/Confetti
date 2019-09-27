using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Confetti.Domain.Uow
{
    /// <summary>
    /// Provides a implementation of the unit of work manager.
    /// </summary>
    public class UnitOfWorkManager : IUnitOfWorkManager
    {
        #region Properties

        protected UnitOfWorkOptions DefaultOptions { get; }
        protected IServiceProvider ServiceProvider { get; }
            
        #endregion

        #region Ctor

        public UnitOfWorkManager(
            IOptions<UnitOfWorkOptions> options,
            IServiceProvider serviceProvider)
        {
            DefaultOptions = options.Value;
            ServiceProvider = serviceProvider;
        }
            
        #endregion

        #region Methods

        /// <summary>
        /// Begins a unit of work with default <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <returns>The uow complete handle.</returns>
        public virtual IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(DefaultOptions);
        }
        
        /// <summary>
        /// Begins a unit of work with <see cref="UnitOfWorkOptions"/>.
        /// </summary>
        /// <param name="options">The <see cref="UnitOfWorkOptions"/>.</param>
        /// <returns>The uow complete handle.</returns>
        public virtual IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            if (options == null) throw new ArgumentNullException(nameof(options));

            var uow = ServiceProvider.GetRequiredService<IUnitOfWork>();
            uow.Begin(options);

            return uow;
        }
            
        #endregion
    }
}