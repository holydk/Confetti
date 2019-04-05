using System.Collections.Generic;

namespace Confetti.Api.Areas.Public.Models.Catalog
{
    /// <summary>
    /// Represents a top menu
    /// </summary>
    public class TopMenuModel
    {
        public IList<CategorySimpleModel> Categories { get; set; }
    }
}