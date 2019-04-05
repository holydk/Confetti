using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Models.Common;

namespace Confetti.Api.Areas.Public.Factories
{
    /// <summary>
    /// Represents the interface of the common models factory
    /// </summary>
    public partial interface ICommonModelFactory
    {
        /// <summary>
        /// Prepare the base layout model
        /// </summary>
        /// <returns>layout model</returns>
        Task<LayoutModel> PrepareLayoutModelAsync();
    }
}