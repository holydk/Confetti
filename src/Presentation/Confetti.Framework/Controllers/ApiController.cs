using Confetti.Framework.Models;
using Microsoft.AspNetCore.Mvc;

namespace Confetti.Framework.Controllers
{
    /// <summary>
    /// Base api controller
    /// </summary>
    public abstract class ApiController : ControllerBase
    {
        public override OkObjectResult Ok(object value)
        {           
            return base.Ok(new ApiResponse
            {
                Response = value
            });
        }
    }
}