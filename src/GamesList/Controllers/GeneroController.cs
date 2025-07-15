using GamesList.Models;
using GamesList.Services.GeneroService;
using Microsoft.AspNetCore.Mvc;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroController(IGeneroService generoService, ILogger<GeneroController> logger) : ApiControllerBase<GeneroController>(logger)
    {
        private readonly IGeneroService _generoService = generoService;


        [HttpGet("list")]
        public async Task<IActionResult> ListGenerosAsync()
        {
            var result = await _generoService.ListGenerosAsync();
            if (!result.Success) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Data);
        }
        [HttpGet()]
        public async Task<IActionResult> ListGenerosByIdsAsync([FromBody] List<int> ids)
        {
            var result = await _generoService.ListGenerosByIdsAsync(ids);
            if (!result.Success) return StatusCode(result.StatusCode, result.Message);
            return Ok(result.Data);
        }
    }

}