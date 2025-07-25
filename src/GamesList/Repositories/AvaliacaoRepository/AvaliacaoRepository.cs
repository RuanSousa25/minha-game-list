using GamesList.Databases;
using GamesList.Dtos.Responses;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.AvaliacaoRepository
{
    public class AvaliacaoRepository(AppDbContext appDbContext) : IAvaliacaoRepository
    {
          private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddAvaliacaoAsync(Avaliacao avaliacao)
        {
            await _appDbContext.Avaliacoes.AddAsync(avaliacao);
        }
        public async Task<Avaliacao?> GetAvaliacaoByIdAsync(int id)
        {
            return await _appDbContext.Avaliacoes.FindAsync(id);
        }

        public async Task<Avaliacao?> GetAvaliacaoByUsuarioIdAndJogoIdAsync(int usuarioId, int jogoId)
        {
            return await _appDbContext.Avaliacoes.Include(a => a.Usuario).Include(a => a.Jogo)
            .SingleOrDefaultAsync(a => a.Usuario.Id == usuarioId && a.Jogo.Id == jogoId);
        }

        public async Task<List<Avaliacao>> GetAvaliacoesByJogoIdAsync(int id)
        {
            return await _appDbContext.Avaliacoes.Include(a => a.Jogo).Include(a => a.Usuario).Where(a => a.Jogo.Id == id).ToListAsync();
        }

        public async Task<List<Avaliacao>> GetAvaliacoesByUsuarioIdAsync(int id)
        {
             return await _appDbContext.Avaliacoes.Include(a => a.Usuario).Include(a => a.Jogo).Where(a => a.Usuario.Id == id).ToListAsync();
        }

        public void RemoveAvaliacao(Avaliacao avaliacao)
        {
            _appDbContext.Avaliacoes.Remove(avaliacao);
        }

        public void RemoveAvaliacoes(List<Avaliacao> avaliacoes)
        {
            _appDbContext.Avaliacoes.RemoveRange(avaliacoes);
        }

        public void UpdateAvaliacao(Avaliacao avaliacao)
        {
            _appDbContext.Avaliacoes.Update(avaliacao);
        }
    }
}