using Confetti.Application.Models.Catalog;
using Confetti.Domain.Catalog;
using Microsoft.Extensions.DependencyInjection;

namespace Confetti.Infrastructure.Mapping.Catalog
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public class CategoryProfile : AutoMapperOrderedProfile
    {
        public override void Configure(IServiceCollection services)
        {
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();

            base.Configure(services);
        }
    }
}
