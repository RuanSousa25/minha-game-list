using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.ImagensSugestaoRepository
{
    public class ImagensSugestaoRepository(AppDbContext appDbContext) : IImagensSugestaoRepository
    {
          private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddImagemAsync(ImagensSugestao imagem)
        {
            await _appDbContext.ImagensSugestao.AddAsync(imagem);
        }

        public void RemoveSugestaoImagens(List<ImagensSugestao> imagens)
        {
            _appDbContext.ImagensSugestao.RemoveRange(imagens);
        }
    }
}