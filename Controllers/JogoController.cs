using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/jogos")]
    public class JogoController(JogoService jogoService) : ApiControllerBase
    {
        private readonly JogoService _jogoService = jogoService;


        [HttpGet()]
        public async Task<IActionResult> ListAllJogos()
        {
            var result = await _jogoService.GetJogosList();
            return FromResult(result);
        }


    }
}