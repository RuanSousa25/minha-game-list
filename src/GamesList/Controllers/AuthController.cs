using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Models;
using GamesList.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ApiControllerBase<AuthController>(logger)
    {
        private readonly IAuthService _authService = authService;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            return FromResult( await _authService.RegisterAsync(request));
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            return FromResult( await _authService.LoginAsync(request));
        }
    }
}