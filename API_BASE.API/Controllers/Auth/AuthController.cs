using API_BASE.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace API_BASE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _jwtService;

        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            // ⚠️ Esto es solo un ejemplo fijo. Más adelante puedes validar desde base de datos
            if (request.Username == "admin" && request.Password == "admin123")
            {
                //Aca se puede luego cambiar el rol de manera dinamica, digase un valor de la base de datos
                var token = _jwtService.GenerateToken("1", request.Username, "admin");

                return Ok(new
                {
                    access_token = token,
                    expires_in = 3600
                });
            }

            return Unauthorized(new { message = "Credenciales incorrectas" });
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
