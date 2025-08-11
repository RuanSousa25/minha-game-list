using GamesList.Common.Pagination;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Repositories.AvaliacaoRepository
{
    public interface IAvaliacaoRepository
    {
        public Task<Avaliacao?> GetAvaliacaoByIdAsync(int id);
        public Task<List<Avaliacao>> GetAvaliacoesByUsuarioIdAsync(int usuarioId);
        public Task<PagedResult<AvaliacaoDto>> GetAvaliacoesByUsuarioIdPagedAsync(int usuarioId, PaginationParams paginationParams);
        public Task<List<Avaliacao>> GetAvaliacoesByJogoIdAsync(int jogoId);
         public Task<PagedResult<AvaliacaoDto>> GetAvaliacoesByJogoIdPagedAsync(int jogoId, PaginationParams paginationParams);
        public void RemoveAvaliacao(Avaliacao avaliacao);
        public void RemoveAvaliacoes(List<Avaliacao> avaliacoes);
        public Task<Avaliacao?> GetAvaliacaoByUsuarioIdAndJogoIdAsync(int usuarioId, int jogoId);
        public Task AddAvaliacaoAsync(Avaliacao avaliacao);
        public void UpdateAvaliacao(Avaliacao avaliacao);

    }
}