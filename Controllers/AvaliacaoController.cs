using GamesList.DTOs.Requests;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{

    [ApiController]
    [Route("api/avaliacoes")]
    public class AvaliacaoController(AvaliacaoService avaliacaoService) : ControllerBase
    {
        private readonly AvaliacaoService _avaliacaoService = avaliacaoService;

        [HttpGet("jogo/{id}")]
        public async Task<ActionResult> GetAvaliacoesByJogoId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByJogoId(id);
            return Ok(result.Data);
        }
        [HttpGet("usuario/{id}")]
        public async Task<ActionResult> GetAvaliacoesByUsuarioId([FromRoute] int id)
        {
            var result = await _avaliacaoService.GetAvaliacoesByUsuarioId(id);
            return Ok(result.Data);
        }
        [HttpPost("jogo")]
        [Authorize]
        public async Task<ActionResult> PostAvaliacao([FromBody] AvaliacaoRequest request)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) throw new Exception("Usuário não autenticado.");

            var result = await _avaliacaoService.SaveAvaliacao(userId, request);
            Console.WriteLine(result.Message);
            if (!result.Success) return StatusCode(500, result.Message);
            return Ok(result.Data);
        }
    }
}