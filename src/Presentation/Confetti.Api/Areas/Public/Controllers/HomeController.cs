using System.Threading.Tasks;
using Confetti.Api.Areas.Public.Factories;
using Confetti.Api.Controllers;
using Confetti.Application.Caching;
using Confetti.Application.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace Confetti.Api.Areas.Public.Controllers
{
    public class HomeController : ApiController
    {
        #region Fields

        private readonly ICommonModelFactory _commonModelFactory;
        private readonly IStaticCacheManager _staticCacheManager;

        #endregion

        #region Ctor

        public HomeController(
            ICommonModelFactory commonModelFactory,
            IStaticCacheManager staticCacheManager
        )
        {
            _commonModelFactory = commonModelFactory;
            _staticCacheManager = staticCacheManager;
        }
        
        #endregion

        [HttpGet]
        [Route("home")]
        public async Task<IActionResult> IndexAsync()
        {
            var layoutModel = await _commonModelFactory.PrepareLayoutModelAsync();
            return Response(layoutModel);
        }

        [HttpGet]
        [Route("cache/clear")]
        public IActionResult ClearCache()
        {
            _staticCacheManager.Clear();
            return Ok();
        }
    }
}