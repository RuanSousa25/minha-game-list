using System.Text.Json;
using GamesList.Common.Pagination;
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
    public class SugerirJogoController(ISugestoesJogoService sugestoesJogoService, ILogger<SugerirJogoController> logger) : ApiControllerBase<SugerirJogoController>(logger)
    {
        private readonly ISugestoesJogoService _sugestoesJogoService = sugestoesJogoService;
        private static readonly JsonSerializerOptions _jsonOptions = new() { PropertyNameCaseInsensitive = true };


        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListSugestaoJogo([FromQuery] PaginationParams paginationParams)
        {
            var result = await _sugestoesJogoService.ListSugerirJogoPagedAsync(paginationParams);
            return FromResult(result);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> ListSugestoesJogoByUserId([FromQuery] PaginationParams paginationParams, [FromRoute] int userId)
        {
            Console.WriteLine(paginationParams.Search == null? "null":"não null");
            Console.WriteLine(userId);
            var result = await _sugestoesJogoService.ListSugerirJogoByUserIdPagedAsync(paginationParams, userId);
            return FromResult(result);
        }


        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> SaveSugestaoJogo([FromForm] string sugestao, [FromForm] IFormFile imagemCapa, [FromForm] IFormFile imagemIcone, [FromForm] List<IFormFile> imagensPromo)
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
            if (imagemCapa == null) return BadRequest("Não há imagem de capa para a sugestão.");
             if (imagemIcone == null) return BadRequest("Não há imagem de ícone para a sugestão.");

            if (GetUserId() is not int userId) return Unauthorized();

            var result = await _sugestoesJogoService.SaveSugestaoJogoComImagemAsync(request, imagemCapa, imagemIcone, userId);
            return FromResult(result);
        }

        [HttpPost("aprovar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AprovarJogo([FromRoute] int id)
        {
            var usuarioId = GetUserId();
            if (usuarioId == null) return Unauthorized(); 
            var result = await _sugestoesJogoService.AprovarJogoAsync(id, (int)usuarioId);
            return FromResult(result);
        }
        [HttpPost("reprovar/{id}")]
        [Authorize]
        public async Task<IActionResult> ReprovarJogo([FromRoute] int id)
        {
            var sugestaoResult = await _sugestoesJogoService.FindSugestaoJogoAsync(id);
            if (sugestaoResult == null) return NotFound("Sugestão de jogo não encontrada");
            if (!sugestaoResult.Success) return FromResult(sugestaoResult);

            var data = sugestaoResult.Data;
            var userId = ClaimsHelper.GetUserId(User);
            var userRole = ClaimsHelper.GetUserRole(User);
            if (data.UsuarioId != userId && userRole != "admin") return Forbid();
            return FromResult( await _sugestoesJogoService.RemoverSugestaoJogoAsync(id));
        }
        
    }
}