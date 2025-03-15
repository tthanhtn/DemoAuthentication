using Demo.API.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace Demo.API.Services
{
    public class CookiesAccount : ICookiesAccount
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CookiesAccount(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> AuthenticationSignIn(int userId, string fullName)
        {
            var sessionId = Guid.NewGuid().ToString().ToLower();

            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, fullName),
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                    new Claim("SessionId", sessionId)
                };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
            {
                IsPersistent = true
            });

            return sessionId;
        }
    }
}
