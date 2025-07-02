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
    public class SugerirJogoController(SugerirJogoService sugerirJogoService, BlobService blobService, ImagensSugestaoService imagensServices) : ApiControllerBase
    {
        private readonly SugerirJogoService _sugerirJogoService = sugerirJogoService;
        private readonly BlobService _blobService = blobService;
        private readonly ImagensSugestaoService _imagensServices = imagensServices;
        private static readonly JsonSerializerOptions _jsonOptions = new (){ PropertyNameCaseInsensitive = true };


        [HttpGet()]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ListSugestaoJogo()
        {
            var result = await _sugerirJogoService.ListSugerirJogo();
            if (!result.Success) return StatusCode(500, result.Message);
            return Ok(result);
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

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(imagem.FileName)}";
            var blobResult = await _blobService.UploadFileAsync(imagem.OpenReadStream(), fileName, imagem.ContentType);
            if (!blobResult.Success) return StatusCode(blobResult.StatusCode, blobResult.Message);

            if (GetUserId() is not int userId) return Unauthorized();
            var sugestaoResult = await _sugerirJogoService.SaveSugestaoJogo(request, userId);
            if (!sugestaoResult.Success) return StatusCode(sugestaoResult.StatusCode, sugestaoResult.Message);

            var sugestaoImagemResult = await _imagensServices.SaveImagem(sugestaoResult.Data, blobResult.Data!);
            if (!sugestaoImagemResult.Success) return StatusCode(sugestaoImagemResult.StatusCode, sugestaoImagemResult.Message);

            return Ok("Sugestão inserida com sucesso.");
        }

        [HttpPost("aprovar/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> AprovarJogo([FromRoute] int id)
        {
            var result = await _sugerirJogoService.AprovarJogo(id);
            return FromResult(result);
        }
    }
}