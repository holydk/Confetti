using AutoMapper;
using Confetti.Catalog.Application.Models;
using Confetti.Catalog.Domain.Models;

namespace Confetti.Infrastructure.Mapping.Catalog
{
    /// <summary>
    /// Mapper profile registrar interface
    /// </summary>
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, CategoryModel>();
            CreateMap<CategoryModel, Category>();
        }
    }
}
