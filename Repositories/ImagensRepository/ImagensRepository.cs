using System.Threading.Tasks;
using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.ImagensRepository
{
    public class ImagensRepository(AppDbContext appDbContext) : IImagensRepository
    {
          private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddImagemAsync(Imagem imagem)
        {
            await _appDbContext.Imagens.AddAsync(imagem);
        }

        public async Task<List<Imagem>> GetImagensByJogoId(int id)
        {
           return await _appDbContext.Imagens.Where(i => i.JogoId == id).ToListAsync();
        }

        public void RemoveImagens(List<Imagem> imagens)
        {
            _appDbContext.RemoveRange(imagens);
        }
    }
}