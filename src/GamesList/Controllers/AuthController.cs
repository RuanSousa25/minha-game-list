using GamesList.DTOs;
using GamesList.DTOs.Requests;
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
            var result = await _authService.Register(request);
            return FromResult(result);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request);
            return FromResult(result);
        }
    }
}