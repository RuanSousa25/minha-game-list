using GamesList.Databases;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.SugerirJogoRepository
{
    public class SugerirJogoRepository(AppDbContext appDbContext) : ISugerirJogoRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task<SugerirJogo?> GetSugerirJogoComRelacoesByIdAsync(int id)
        {
            return await _appDbContext
            .SugerirJogo
            .Include(s => s.Generos)
            .Include(s => s.Imagens)
            .SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task<SugerirJogo?> GetSugerirJogoByIdAsync(int id)
        {
            return await _appDbContext
            .SugerirJogo
            .FindAsync(id);
        }

        public async Task<List<SugerirJogo>> GetSugerirJogoByUsuarioIdAsync(int id)
        {
            return await _appDbContext.SugerirJogo.Where(s => s.UsuarioId == id).ToListAsync();
        }

        public async Task<List<SugerirJogo>> ListSugerirJogosAsync()
        {
            return await _appDbContext.SugerirJogo.ToListAsync();
        }

        public void RemoveSugestao(SugerirJogo sugerirJogo)
        {
            _appDbContext.SugerirJogo.Remove(sugerirJogo);
        }
        public async Task AddSugerirJogoAsync(SugerirJogo sugerirJogo)
        {
            await _appDbContext.SugerirJogo.AddAsync(sugerirJogo);            
        }
    }
}