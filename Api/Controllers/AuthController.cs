using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public ActionResult<string> Login([FromBody] LoginUserRequest request)
        {
            var token = _authService.Login(request);
            if (token == null)
            {
                return Unauthorized("Credenciales inválidas");
            }
            return Ok(token);
        }
    }
}
