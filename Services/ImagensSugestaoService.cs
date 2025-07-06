using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;
using static GamesList.DTOs.Helpers.Results;
namespace GamesList.Services
{
    public class ImagensSugestaoService(AppDbContext appDbContext, ILogger<ImagensSugestaoService> logger)
    {
        private readonly AppDbContext _appDbContext = appDbContext;
        private readonly ILogger<ImagensSugestaoService> _logger = logger;

        public async Task<ServiceResultDto<string>> SaveImagem(int sugestaoJogoId, string url)
        {
            var imagem = new ImagensSugestao { SugerirJogoId = sugestaoJogoId, Url = url, TipoId = 1 };
            _appDbContext.ImagensSugestao.Add(imagem);
            await _appDbContext.SaveChangesAsync();
            _logger.LogInformation("Imagem {img} inserida com sucesso.", url);
            return Ok("Imagem inserida com sucesso");
        }
    }
}