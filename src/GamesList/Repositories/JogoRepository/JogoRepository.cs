using System.Threading.Tasks;
using GamesList.Common.Pagination;
using GamesList.Databases;
using GamesList.Dtos;
using GamesList.Models;
using Microsoft.EntityFrameworkCore;

namespace GamesList.Repositories.JogoRepository
{
    public class JogoRepository(AppDbContext appDbContext) : IJogoRepository
    {
        private readonly AppDbContext _appDbContext = appDbContext;

        public async Task AddJogoAsync(Jogo jogo)
        {
            await _appDbContext.Jogos.AddAsync(jogo);
        }

        public async Task<bool> CheckIfJogoExistsAsync(int id)
        {
            return await _appDbContext.Jogos.AnyAsync(j => j.Id == id);
        }

        public async Task<Jogo?> GetJogoAsync(int id)
        {
            return await _appDbContext.Jogos
            .Include(j => j.Generos)
            .Include(j => j.Avaliacoes)
            .Include(j => j.Imagens)
            .FirstOrDefaultAsync(j => j.Id == id);
        }

        public async Task<Jogo?> GetJogoComRelacionamentoByIdAsync(int id)
        {
            var jogo =
            await _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .FirstOrDefaultAsync(j => j.Id == id);
            return jogo;
        }

        public async Task<List<Jogo>> GetJogosAsync()
        {
            var jogos =
            await _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .ToListAsync();
            return jogos;
        }

        public async Task<PagedResult<JogoDto>> GetJogosPagedAsync(PaginationParams paginationParams)
        {
            var jogos =
            await _appDbContext.Jogos
            .Include(j => j.Avaliacoes)
            .Include(j => j.Generos)
            .Include(j => j.Imagens)
            .OrderBy(j => j.Nome)
            .Select(j => new JogoDto(j))
            .ToPagedResultAsync(paginationParams);
            return jogos;
        }

        public async Task<bool> RemoveJogoByIdAsync(int id)
        {
            var jogo = await _appDbContext.Jogos.FindAsync(id);
            if (jogo == null) return false;
            _appDbContext.Jogos.Remove(jogo);
            return true;
        }
    }
}