using System.Collections.Generic;

namespace Confetti.Api.Areas.Public.Models.Catalog
{
    public class CategorySimpleModel
    {
        public string Title { get; set; }

        public List<CategorySimpleModel> SubCategories { get; set; }

        public bool HaveSubCategories { get; set; }

        public string Route { get; set; }

        public CategorySimpleModel()
        {
            SubCategories = new List<CategorySimpleModel>();
        }
    }
}