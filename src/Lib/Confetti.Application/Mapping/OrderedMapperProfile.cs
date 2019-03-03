using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Application.Mapping
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public abstract class OrderedMapperProfile : Profile, IOrderedMapperProfile
    {
        #region Properties

        /// <summary>
        /// Gets order of this configuration implementation
        /// </summary>
        public int Order { get; }
            
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="order">order of this configuration implementation</param>
        public OrderedMapperProfile(int order = 0)
        {
            Order = order;
        }
            
        #endregion

        #region Methods

        public virtual void Configure(IServiceCollection services)
        {           
        }

        #endregion
    }
}
