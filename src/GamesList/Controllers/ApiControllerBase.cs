using GamesList.Dtos;
using GamesList.Dtos.Responses;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase<C>(ILogger<C> logger) : ControllerBase
    {
        private readonly ILogger<C> _logger = logger;
        protected IActionResult FromResult<T>(ServiceResultDto<T> result)
        {
            if (result == null)
            {
                _logger.LogError("ServiceResultDto nulo em {controller}.", GetType().Name);
                return StatusCode(500, new MessageResponseDto("Erro interno no servidor."));
            }
            if (!result.Success)
            {
                _logger.LogError("ServiceResultDto falhou em {controller}.", GetType().Name);
                return StatusCode(result.StatusCode == 0 ? 500 : result.StatusCode, new MessageResponseDto(result.Message ?? "Ocorreu um erro desconhecido, tente novamente mais tarde"));
            }

            return StatusCode(result.StatusCode == 0 ? 200 : result.StatusCode, result.Data);
        }

        protected int? GetUserId()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }
}
