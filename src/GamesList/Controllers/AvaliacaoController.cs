using GamesList.DTOs.Helpers;
using GamesList.DTOs.Requests;
using GamesList.Services.AvaliacaoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{

    [ApiController]
    [Route("api/avaliacoes")]
    public class AvaliacaoController(IAvaliacaoService avaliacaoService, ILogger<AvaliacaoController> logger) : ApiControllerBase<AvaliacaoController>(logger)
    {
        private readonly IAvaliacaoService _avaliacaoService = avaliacaoService;


        [HttpGet("jogo/{id}")]
        public async Task<IActionResult> GetAvaliacoesByJogoId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByJogoIdAsync(id);
            return FromResult(result);
        }
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetAvaliacoesByUsuarioId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByUsuarioIdAsync(id);
            return FromResult(result);
        }
        [HttpPost("jogo")]
        [Authorize]
        public async Task<IActionResult> PostAvaliacao([FromBody] AvaliacaoRequest request)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var result = await _avaliacaoService.SaveAvaliacaoAsync(userId, request);
            return FromResult(result);
        }
        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveAvaliacao([FromRoute] int id)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null || !int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var isAdmin = ClaimsHelper.GetUserRole(User) == "admin";
            var result = await _avaliacaoService.RemoveAvaliacaoByIdAsync(id, userId, isAdmin);
            return FromResult(result);
        }
    }
}