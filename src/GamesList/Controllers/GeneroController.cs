using GamesList.Models;
using GamesList.Services.GeneroService;
using Microsoft.AspNetCore.Mvc;
using Sprache;

namespace GamesList.Controllers
{
    [ApiController]
    [Route("api/generos")]
    public class GeneroController(IGeneroService generoService, ILogger<GeneroController> logger) : ApiControllerBase<GeneroController>(logger)
    {
        private readonly IGeneroService _generoService = generoService;



        [HttpGet()]
        public async Task<IActionResult> ListGenerosByIdsAsync([FromQuery] List<int>? id, [FromRoute] string? search)
        {
            if (id is { Count: > 0 })
            {
                return FromResult( await _generoService.ListGenerosByIdsAsync(id));
            }
            else
            {
                return FromResult( await _generoService.ListGenerosAsync(search));
            }            
        }
    }

}