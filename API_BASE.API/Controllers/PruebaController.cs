using Microsoft.AspNetCore.Mvc;

namespace API_BASE.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PruebaController : ControllerBase
    {
        [HttpGet("error")]
        public IActionResult LanzarError()
        {
            throw new Exception("¡Esto es una prueba de excepción global!");
        }
    }
}
