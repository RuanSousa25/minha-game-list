using GamesList.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    public abstract class ApiControllerBase : ControllerBase
    {
        protected IActionResult FromResult<T>(ServiceResultDto<T> result)
        {
            if (result == null)
                return StatusCode(500, "Erro interno no servidor.");

            if (!result.Success)
                return StatusCode(result.StatusCode == 0 ? 500 : result.StatusCode, result.Message);

            return StatusCode(result.StatusCode == 0 ? 200 : result.StatusCode, result.Data);
        }

        protected int? GetUserId()
        {
            var userIdStr = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            return int.TryParse(userIdStr, out var userId) ? userId : null;
        }
    }
}
