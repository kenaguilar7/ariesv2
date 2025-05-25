using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Aries.WebAPI.Controllers
{
    /// <summary>
    /// Base controller for all Aries controllers providing common functionality and attributes.
    /// </summary>
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public abstract class AriesBaseController : ControllerBase
    {
        protected int UserId
        {
            get
            {
                //return int.Parse(User?.FindFirst("UserId")?.Value ?? 
                //       User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
                //       string.Empty);
                return 1; 
            }
        }

        protected IActionResult HandleException(Exception ex)
        {
            Console.WriteLine($"Error: {ex}");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }

        protected IActionResult HandleValidationError()
        {
            return BadRequest(ModelState);
        }
    }
} 