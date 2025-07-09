using ICEDT.API.DTO.Request;
using ICEDT.API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ICEDT.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
           

            var (success, token, errorMessage) = await _authService.LoginAsync(loginDto);
            if (!success)
            {
                return Unauthorized(new { Error = errorMessage });
            }

            return Ok(new { Token = token });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
        

            var (success, errorMessage) = await _authService.RegisterAsync(registerDto);
            if (!success)
            {
                return BadRequest(new { Error = errorMessage });
            }

            return Ok(new { Message = "Registration successful" });
        }
    }
}
