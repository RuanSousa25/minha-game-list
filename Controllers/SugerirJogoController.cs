using System.Text.Json;
using GamesList.DTOs.Requests;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/sugerirjogo")]
    public class SugerirJogoController(SugerirJogoService sugerirJogoService, BlobService blobService, ImagensSugestaoService imagensServices) : ControllerBase
    {
        private readonly SugerirJogoService _sugerirJogoService = sugerirJogoService;
        private readonly BlobService _blobService = blobService;
        private readonly ImagensSugestaoService _imagensServices = imagensServices;


        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ListSugestaoJogo()
        {
            var result = await _sugerirJogoService.ListSugerirJogo();
            if (!result.Success) return StatusCode(500, result.Message);
            return Ok(result);
        }

        [HttpPost()]
        [Authorize]
        public async Task<ActionResult<string>> SaveSugestaoJogo([FromForm] string sugestao, [FromForm] IFormFile imagem)
        {
            if (string.IsNullOrWhiteSpace(sugestao)) return BadRequest("Campo de sugestão vazio.");

            UploadGameRequest request;
            try
            {
                request = JsonSerializer.Deserialize<UploadGameRequest>(sugestao);
            }
            catch
            {
                return BadRequest("Sugestão inválida. JSON não correspondente.");
            }

            if (imagem == null) return BadRequest("Não há imagem para a sugestão.");

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
            var blobResult = await _blobService.UploadFileAsync(imagem.OpenReadStream(), fileName);
            if (!blobResult.Success) return StatusCode(500, blobResult.Message);

            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(userIdStr, out var userId)) throw new Exception("Usuário não autenticado.");

            var sugestaoResult = await _sugerirJogoService.SaveSugestaoJogo(request, userId);
            if (!sugestaoResult.Success) return StatusCode(500, sugestaoResult.Message);
            var sugestaoImagemResult = await _imagensServices.SaveImagem(sugestaoResult.Data, blobResult.Data);
            if (!sugestaoImagemResult.Success) return StatusCode(500, sugestaoImagemResult.Message);

            return Ok("Sugestão inserida com sucesso.");
        }

        [HttpPost("aprovar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> AprovarJogo([FromRoute] int id)
        {
            var result = await _sugerirJogoService.AprovarJogo(id);
            if (!result.Success) return StatusCode(500, result.Message);

            return Ok(result.Data);
        }
    }
}