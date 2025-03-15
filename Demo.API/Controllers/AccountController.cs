using Demo.API.Interface;
using Demo.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.API.Controllers
{
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ICookiesAccount _userAccount;
        public AccountController(ICookiesAccount userAccount) {
            _userAccount = userAccount;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginViewModel model)
        {
            User user = GetUserInformation(model);

            if(user.Id == 0)
            {
                return BadRequest("Invalid username or password");
            }  
            
            string sessionId = await _userAccount.AuthenticationSignIn(user.Id, model.Password);

            return Ok($"This is an LoginAsync sessionId:  {sessionId}");
        }

        [HttpGet("GetInforUser")]
        [Authorize]
        public IActionResult GetInforUser(int userId)
        {
            return Ok("GetInforUser after login");
        }


        private User GetUserInformation(LoginViewModel model)
        {
            // Logic check user in database

            return new User { Id = 1 };
        }
    }
}
