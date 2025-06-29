using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController(AuthService authService, IConfiguration config) : ControllerBase
    {
        private readonly AuthService _authService = authService;
        private readonly IConfiguration _configuration = config;


        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            var result = await _authService.Register(request);
            if (!result.Success) return BadRequest(result.Message);

            return Ok(result.Data);
        }
        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.Login(request);
            if (!result.Success) return Unauthorized(result.Message);

            return Ok(result.Data);
        }
    }
}