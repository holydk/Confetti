using Confetti.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Confetti.Api.Controllers
{
    /// <summary>
    /// Base api controller
    /// </summary>
    public abstract class ApiController : ControllerBase
    {
        protected new IActionResult Response(object result = null)
        {
            if (ModelState.IsValid)
            {
                return Ok(new ApiResponse
                {
                    Response = result
                });
            }

            return BadRequest(new
            {
                errors = new[] 
                {
                    "BadRequest"
                }
            });
        }
    }
}