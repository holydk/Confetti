using Confetti.Application.Models.Catalog;
using Confetti.Domain.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Infrastructure.Mapping.Catalog
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public class ProductProfile : AutoMapperOrderedProfile
    {
        public override void Configure(IServiceCollection services)
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductModel, Product>();

            base.Configure(services);
        }
    }
}
