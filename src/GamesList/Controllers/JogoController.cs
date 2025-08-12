using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Models;
using GamesList.Services.JogoService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/jogos")]
    public class JogoController(IJogoService jogoService, ILogger<JogoController> logger) : ApiControllerBase<JogoController>(logger)
    {
        private readonly IJogoService _jogoService = jogoService;


        [HttpGet()]
        public async Task<IActionResult> GetJogosPagedAsync([FromQuery] PaginationParams paginationParams)
        {
            return FromResult(await _jogoService.GetJogosPagedAsync(paginationParams));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetJogoAsync([FromRoute] int id)
        {
            return FromResult(await _jogoService.GetJogoDtoAsync(id));
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteJogoAsync([FromRoute] int id)
        {
            return FromResult(await _jogoService.RemoveJogoAsync(id));
        }
        
    }
}