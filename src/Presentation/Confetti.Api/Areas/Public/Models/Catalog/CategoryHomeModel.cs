using System.Collections.Generic;
using Confetti.Application.Models.Catalog;

namespace Confetti.Api.Areas.Public.Models.Catalog
{
    public class CategoryHomeModel
    {
        /// <summary>
        /// Gets or sets the title
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the meta title
        /// </summary>
        public string MetaTitle { get; set; }

        /// <summary>
        /// Gets or sets the meta description
        /// </summary>
        public string MetaDescription { get; set; }

        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Categories are displayed on home page
        /// </summary>
        public List<CategoryPublicModel> Categories { get; set; }

        public CategoryHomeModel()
        {
            Categories = new List<CategoryPublicModel>();
        }
    }
}