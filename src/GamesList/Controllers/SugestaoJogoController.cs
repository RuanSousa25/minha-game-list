using System.Text.Json;
using GamesList.Dtos.Helpers;
using GamesList.Dtos.Requests;
using GamesList.Services.SugestoesJogoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/sugerirjogo")]
    public class SugerirJogoController(ISugestoesJogoService sugerirJogoService, ILogger<SugerirJogoController> logger) : ApiControllerBase<SugerirJogoController>(logger)
    {
        private readonly ISugestoesJogoService _sugerirJogoService = sugerirJogoService;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };


        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListSugestaoJogo()
        {
            var result = await _sugerirJogoService.ListSugerirJogoAsync();
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

            var result = await _sugerirJogoService.SaveSugestaoJogoComImagemAsync(request, imagem, userId);
            return FromResult(result);
        }

        [HttpPost("aprovar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AprovarJogo([FromRoute] int id)
        {
            var result = await _sugerirJogoService.AprovarJogoAsync(id);
            return FromResult(result);
        }
        [HttpPost("reprovar/{id}")]
        [Authorize]
        public async Task<IActionResult> ReprovarJogo([FromRoute] int id)
        {
            var sugestaoResult = await _sugerirJogoService.FindSugestaoJogoAsync(id);
            if (sugestaoResult == null) return NotFound("Sugestão de jogo não encontrada");
            if (!sugestaoResult.Success) return FromResult(sugestaoResult);

            var data = sugestaoResult.Data;
            var userId = ClaimsHelper.GetUserId(User);
            var userRole = ClaimsHelper.GetUserRole(User);
            if (data.UsuarioId != userId && userRole != "admin") return Forbid();
            return FromResult( await _sugerirJogoService.RemoverSugestaoJogoAsync(id));
        }
        
    }
}