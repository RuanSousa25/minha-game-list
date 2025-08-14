using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Models;

namespace GamesList.Repositories.SugestoesJogoRepository
{
    public interface ISugestoesJogoRepository
    {
        public Task<PagedResult<SugestaoJogoDto>> ListSugestoesJogosPagedAsync(PaginationParams paginationParams);
        public Task<SugestaoJogo?> GetSugestaoJogoComRelacoesByIdAsync(int id);
        public Task<SugestaoJogo?> GetSugestaoJogoByIdAsync(int id);
        public Task<List<SugestaoJogo>> GetSugestoesJogoByUsuarioIdAsync(int id);
        public void RemoveSugestao(SugestaoJogo sugerirJogo);
        public Task AddSugestaoJogoAsync(SugestaoJogo sugerirJogo);
    }
}