using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Models;

namespace GamesList.Repositories.JogoRepository
{
    public interface IJogoRepository
    {

        public Task<List<Jogo>> GetJogosAsync();
        public Task<PagedResult<JogoDto>> GetJogosPagedAsync(PaginationParams paginationParams);
        public Task<Jogo?> GetJogoComRelacionamentoByIdAsync(int id);
        public Task<bool> RemoveJogoByIdAsync(int id);
        public Task<bool> CheckIfJogoExistsAsync(int id);
        public Task AddJogoAsync(Jogo jogo);
        public Task<Jogo?> GetJogoAsync(int id);
    }
}