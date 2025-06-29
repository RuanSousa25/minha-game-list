using GamesList.DTOs;
using GamesList.DTOs.Requests;
using GamesList.Models;
using GamesList.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("game")]
    public class JogoController(JogoService jogoService) : ControllerBase
    {
        private readonly JogoService _jogoService = jogoService;


        [HttpGet()]
        public async Task<ActionResult> ListAllJogos()
        {
            var result = _jogoService.GetJogosList();
            return Ok(result);
        }
    
    }
}