using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Abstraction.ExternalServices;
using Contracts.User.Request;

namespace TheFrogGames.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<string> Login([FromBody] LoginUserRequest request)
        {
            var token = _authService.Login(request);
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Credenciales inválidas");
            }
            return Ok(token);
        }
    }
}
