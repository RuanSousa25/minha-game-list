using GamesList.Dtos.Helpers;
using GamesList.Dtos.Requests;
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
        public async Task<IActionResult> GetAvaliacoesByJogoIdAsync([FromRoute] int id)
        {
            return FromResult(await _avaliacaoService.GetAvaliacoesByJogoIdAsync(id));
        }
        [HttpGet("usuario/{id}")]
        public async Task<IActionResult> GetAvaliacoesByUsuarioIdAsync([FromRoute] int id)
        {
            return FromResult(await _avaliacaoService.GetAvaliacoesByUsuarioIdAsync(id));
        }
        [HttpPost("jogo")]
        [Authorize]
        public async Task<IActionResult> PostAvaliacaoAsync([FromBody] AvaliacaoRequest request)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) return Unauthorized();
            return FromResult(await _avaliacaoService.SaveAvaliacaoAsync(userId, request));
        }
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> RemoveAvaliacaoAsync([FromRoute] int id)
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (userIdStr == null || !int.TryParse(userIdStr, out var userId)) return Unauthorized();
            var isAdmin = ClaimsHelper.GetUserRole(User) == "admin";
            return FromResult( await _avaliacaoService.RemoveAvaliacaoByIdAsync(id, userId, isAdmin));
        }
    }
}