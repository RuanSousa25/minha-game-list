using GamesList.Databases;
using GamesList.DTOs;
using Microsoft.EntityFrameworkCore;
using static GamesList.DTOs.Helpers.Results;

namespace GamesList.Services.ImagensService
{
    public class ImagensServices(AppDbContext appDbContext, ILogger<ImagensServices> logger) : IImagensService
    {
         private readonly ILogger<ImagensServices> _logger = logger;
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<string>> RemoveImagensByJogoId(int id)
        {
            var imagens = await _appDbContext.Imagens.Where(i => i.JogoId == id).ToListAsync();
            _appDbContext.RemoveRange(imagens);
            _logger.LogInformation("Realizada a remoção de {lenght} imagens do jogo {id}", imagens.Count, id);
            return Ok("Remoção de imagens realizada realizada.");

        }
    }
}