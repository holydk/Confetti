using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Factories;
using Confetti.Api.Areas.Public.Models.Common;

namespace Confetti.Api.Areas.Public.Factories
{
    /// <summary>
    /// Represents the common models factory
    /// </summary>
    public partial class CommonModelFactory : ICommonModelFactory
    {
        #region Fields

        private readonly ICatalogModelFactory _catalogModelFactory;
            
        #endregion

        #region Ctor

        public CommonModelFactory(
            ICatalogModelFactory catalogModelFactory
        )
        {
            _catalogModelFactory = catalogModelFactory;
        }
        
        #endregion

        #region Methods

        /// <summary>
        /// Prepare the home model
        /// </summary>
        /// <returns>Home model</returns>
        public async Task<LayoutModel> PrepareLayoutModelAsync()
        {
            var header = new HeaderModel()
            {
                TopMenuModel = await _catalogModelFactory.PrepareTopMenuModelAsync()
            };
            var layout = new LayoutModel()
            {
                HeaderModel = header
            };

            return layout;
        }
        
        #endregion
    }
}