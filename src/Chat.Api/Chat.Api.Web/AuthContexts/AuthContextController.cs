using System.Linq;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Chat.Api.Web.AuthContexts
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    [Authorize]
    [Route("api")]
    public class AuthContextController : ControllerBase
    {
        [HttpGet]
        [Route("GetAuthContext")]
        public IActionResult GetAuthContext()
        {
            var userId = this.User.FindFirstValue(JwtClaimTypes.Subject);
            var email = this.User.FindFirstValue(JwtClaimTypes.Email);
            var profile = new UserProfile{Email = email };
            var context = new AuthContext
            {
                UserProfile = profile,
                Claims = User.Claims.Select(c => new SimpleClaim { Type = c.Type, Value = c.Value }).ToList()
            };
            return Ok(context);
        }
    }
}
