using Confetti.Application.Models.Catalog;
using Confetti.Domain.Core.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Application.Mapping.Catalog
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public class ProductProfile : OrderedMapperProfile
    {
        public override void Configure(IServiceCollection services)
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            base.Configure(services);
        }
    }
}
