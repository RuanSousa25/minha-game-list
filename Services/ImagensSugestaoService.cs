using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.DTOs;
using GamesList.Models;

namespace GamesList.Services
{
    public class ImagensSugestaoService(AppDbContext appDbContext)
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<ServiceResultDto<string>> SaveImagem(int sugestaoJogoId, string url)
        {
            var imagem = new ImagensSugestao{ SugerirJogoId = sugestaoJogoId, Url = url, TipoId = 1 };
            _appDbContext.ImagensSugestao.Add(imagem);
            await _appDbContext.SaveChangesAsync();
            return ServiceResultDto<string>.Ok("Imagem inserida com sucesso");
        }
    }
}