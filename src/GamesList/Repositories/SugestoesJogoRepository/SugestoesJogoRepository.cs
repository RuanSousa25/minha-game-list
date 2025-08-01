using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.SugestoesJogoRepository
{
    public class SugestoesJogoRepository(AppDbContext appDbContext) : ISugestoesJogoRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<SugestaoJogo?> GetSugestaoJogoComRelacoesByIdAsync(int id)
        {
            return await _appDbContext
            .SugestoesJogo
            .Include(s => s.Generos)
            .Include(s => s.Imagens)
            .SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<SugestaoJogo?> GetSugestaoJogoByIdAsync(int id)
        {
            return await _appDbContext
            .SugestoesJogo
            .FindAsync(id);
        }

        public async Task<List<SugestaoJogo>> GetSugestoesJogoByUsuarioIdAsync(int id)
        {
            return await _appDbContext.SugestoesJogo.Include(s => s.Generos).Include(s => s.Imagens).Where(s => s.UsuarioId == id).ToListAsync();
        }

        public async Task<List<SugestaoJogo>> ListSugestoesJogosAsync()
        {
            return await _appDbContext.SugestoesJogo.Include(s => s.Generos).Include(s => s.Imagens).ToListAsync();
        }

        public void RemoveSugestao(SugestaoJogo SugestoesJogo)
        {
            _appDbContext.SugestoesJogo.Remove(SugestoesJogo);
        }
        public async Task AddSugestaoJogoAsync(SugestaoJogo SugestoesJogo)
        {
            await _appDbContext.SugestoesJogo.AddAsync(SugestoesJogo);            
        }
    }
}