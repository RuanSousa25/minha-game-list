using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.SugestoesImagemRepository
{
    public class SugestoesImagemRepository(AppDbContext appDbContext) : ISugestoesImagemRepository
    {
          private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddImagemAsync(SugestaoImagem imagem)
        {
            await _appDbContext.SugestoesImagem.AddAsync(imagem);
        }

        public void RemoveSugestaoImagens(List<SugestaoImagem> imagens)
        {
            _appDbContext.SugestoesImagem.RemoveRange(imagens);
        }
    }
}