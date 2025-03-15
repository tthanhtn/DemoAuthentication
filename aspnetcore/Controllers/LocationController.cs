using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace aspnetcore.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
        [HttpGet("GetValueAnonymous")]
        [AllowAnonymous]
        public IActionResult GetValueAnonymous()
        {
            return Ok("This is an anonymous value");
        }
    }
}

