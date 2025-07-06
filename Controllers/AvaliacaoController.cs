using GamesList.DTOs.Requests;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{

    [ApiController]
    [Route("api/avaliacoes")]
    public class AvaliacaoController(AvaliacaoService avaliacaoService, ILogger<AvaliacaoController> logger) : ApiControllerBase<AvaliacaoController>(logger)
    {
        private readonly AvaliacaoService _avaliacaoService = avaliacaoService;


        [HttpGet("jogo/{id}")]
        public async Task<IActionResult> GetAvaliacoesByJogoId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByJogoId(id);
             return FromResult(result);
        }
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetAvaliacoesByUsuarioId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByUsuarioId(id);
            return FromResult(result);
        }
        [HttpPost("jogo")]
        [Authorize]
        public async Task<IActionResult> PostAvaliacao([FromBody] AvaliacaoRequest request)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var result = await _avaliacaoService.SaveAvaliacao(userId, request);
            return FromResult(result);
        }
    }
}