using GamesList.Common.Pagination;
using GamesList.Dtos;
using GamesList.Dtos.Requests;
using GamesList.Dtos.Responses;
using GamesList.Models;

namespace GamesList.Services.AvaliacaoService
{
    public interface IAvaliacaoService
    {
        public Task<ServiceResultDto<Avaliacao>> GetAvaliacaoByIdAsync(int id);
        public Task<ServiceResultDto<PagedResult<AvaliacaoDto>>> GetAvaliacoesByJogoIdPagedAsync(int jogoId, PaginationParams paginationParams);
        public Task<ServiceResultDto<PagedResult<AvaliacaoDto>>> GetAvaliacoesByUsuarioIdPagedAsync(int usuarioId, PaginationParams paginationParams);
        public Task<ServiceResultDto<AvaliacaoDto>> SaveAvaliacaoAsync(int usuarioId, AvaliacaoRequest request);
        public Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacoesByJogoIdAsync(int jogoId);
        public Task<ServiceResultDto<MessageResponseDto>> RemoveAvaliacaoByIdAsync(int id, int usuarioId, bool isAdmin);
    }
}