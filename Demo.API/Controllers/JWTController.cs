using Demo.API.Interface;
using Demo.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JWTController : ControllerBase
    {
        private readonly IJwtAccount _jWT;
        public JWTController(IJwtAccount jWT) {
            _jWT = jWT;
        }

        [HttpPost("LoginJWT")]
        public IActionResult LoginJWT([FromBody] LoginViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                return BadRequest("Invalid username or password");
            }

            var tokenString = _jWT.AuthenticationSignIn(1, model.UserName).Result;

            return Ok(new { Token = tokenString });
        }


        [HttpGet("GetInforUserJwt")]
        [Authorize]
        public IActionResult GetInforUserJwt(int userId)
        {
            return Ok("GetInforUser after login JWT");
        }

    }
}
