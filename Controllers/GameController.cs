using GamesList.Models;
using GamesList.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [Route("api/games")]
    [ApiController]
    public class GameController(GameService gameService) : ControllerBase
    {
        private GameService _gameService = gameService;

        [HttpGet("")]
        public async Task<ActionResult<List<Game>>> ListGames()
        {
            var result = await _gameService.GetAllGames();

            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameById([FromRoute()] int id)
        {
            var result = await _gameService.FindGame(id);

            return result is Game game ? Ok(game) : NotFound();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Game>> DeleteGameById([FromRoute()] int id)
        {
            var result = await _gameService.DeleteGame(id);
            return result is Game game ? Ok(game) : NotFound();
        }

        [HttpPost("")]
        public async Task<ActionResult<Game>> SaveGame([FromBody()] Game game)
        {
            var result = await _gameService.SaveGame(game);
            return Ok(result);
        }

        [HttpPut()]
        public async Task<ActionResult<Game?>> UpdateGame([FromBody()] Game game)
        {
            var result = await _gameService.UpdateGame(game);
            return Ok(result);
        }


    }
}