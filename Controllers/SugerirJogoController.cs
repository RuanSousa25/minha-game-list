using System.Text.Json;
using GamesList.DTOs.Helpers;
using GamesList.DTOs.Requests;
using GamesList.Services.SugerirJogoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/sugerirjogo")]
    public class SugerirJogoController(ISugerirJogoService sugerirJogoService, ILogger<SugerirJogoController> logger) : ApiControllerBase<SugerirJogoController>(logger)
    {
        private readonly ISugerirJogoService _sugerirJogoService = sugerirJogoService;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };


        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListSugestaoJogo()
        {
            var result = await _sugerirJogoService.ListSugerirJogo();
            return FromResult(result);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> SaveSugestaoJogo([FromForm] string sugestao, [FromForm] IFormFile imagem)
        {
            if (string.IsNullOrWhiteSpace(sugestao)) return BadRequest("Campo de sugestão vazio.");

            UploadGameRequest? request;
            try
            {
                request = JsonSerializer.Deserialize<UploadGameRequest>(sugestao, _jsonOptions);
            }
            catch
            {
                return BadRequest("Sugestão inválida. JSON não correspondente.");
            }

            if (request == null) return BadRequest("Não há sugestão para inserir.");
            if (imagem == null) return BadRequest("Não há imagem para a sugestão.");
            if (GetUserId() is not int userId) return Unauthorized();

            var result = await _sugerirJogoService.SaveSugestaoJogoComImagem(request, imagem, userId);
            return FromResult(result);
        }

        [HttpPost("aprovar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AprovarJogo([FromRoute] int id)
        {
            var result = await _sugerirJogoService.AprovarJogo(id);
            return FromResult(result);
        }
        [HttpPost("reprovar/{id}")]
        [Authorize]
        public async Task<IActionResult> ReprovarJogo([FromRoute] int id)
        {
            var sugestaoResult = await _sugerirJogoService.FindSugestaoJogo(id);
            if (sugestaoResult == null) return NotFound("Sugestão de jogo não encontrada");
            if (!sugestaoResult.Success) return FromResult(sugestaoResult);

            var data = sugestaoResult.Data;
            var userId = ClaimsHelper.GetUserId(User);
            var userRole = ClaimsHelper.GetUserRole(User);
            if (data.UsuarioId != userId && userRole != "admin") return Forbid();

            var result = await _sugerirJogoService.RemoverSugestaoJogo(id);
            return FromResult(result);
        }
        
    }
}