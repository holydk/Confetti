using Confetti.Application.Models.Catalog;
using Confetti.Domain.Core.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Application.Mapping.Catalog
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public class CategoryProfile : OrderedMapperProfile
    {
        public override void Configure(IServiceCollection services)
        {
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();

            base.Configure(services);
        }
    }
}
