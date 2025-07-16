using GamesList.DTOs;
using GamesList.DTOs.Requests;
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
        public async Task<IActionResult> ListAllJogos()
        {
            return FromResult( await _jogoService.ListJogosAsync());
        }
        
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteJogo([FromRoute] int id)
        {
            return FromResult( await _jogoService.RemoveJogoAsync(id));
        }
    }
}