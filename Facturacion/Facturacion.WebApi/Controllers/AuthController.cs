using Facturacion.Application.Dtos.Auth;
using Facturacion.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Facturacion.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService  )
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
            => Ok(await _authService.LoginAsync(loginDto));

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);
            return StatusCode(201, new { mensaje = "Usuario registrado correctamente" });
        }
    }
}
